using TechTalk.SpecFlow;
using System;
using NUnit.Framework;
using Webapi.Domain.Repositorys.Concrete;
using Microsoft.AspNetCore.Mvc;
using Webapi.Enums;
using System.Linq;
using Webapi.Domain.Services.Concrete;
using Webapi.Controllers;
using Webapi.Entities;
using System.Collections.Generic;

namespace Test.src.steps
{
    [Binding]
    public class PersonalizarPizzaSteps
    {
        private MassaDadosBuilder MassaBuilder { get; set; }
        private IActionResult Result { get; set; }
        private void Inicializar()
        {
            this.MassaBuilder = new MassaDadosBuilder();
            this.MassaBuilder.Inicializar();
         }

        private void Destruir()
        {
            this.MassaBuilder.Destruir();
        }

        [Given(@"um cliente que montou sua pizza com tamanho e sabor")]
        public void GivenUmClienteQueMontouSuaPizzaComTamanhoESabor()
        {
            Inicializar();

           this.MassaBuilder.MontarESalvarItemPreco((int)TipoItemPreco.ModoPreparo, 40)
                            .MontarESalvarSabor("Baiana")
                            .MontarESalvarModoPreparo(this.MassaBuilder.ItensPrecos.First().Id, (int)TamanhoPizza.Grande, this.MassaBuilder.Sabor.Id) 
                            .MontarPedido(this.MassaBuilder.ItensPrecos.First().Valor, this.MassaBuilder.Sabor.Id, this.MassaBuilder.ModoPreparo);           
        }

        [Given(@"que personalizou seu pedido adicionando itens tornando única pizza")]
        public void GivenQuePersonalizouSeuPedidoAdicionandoItensTornandoUnicaPizza()
        {    
           this.MassaBuilder.MontarESalvarPedido(this.MassaBuilder.ItensPrecos.First().Valor, this.MassaBuilder.Sabor.Id, this.MassaBuilder.ModoPreparo)                 
                            .MontarESalvarItemPreco((int)TipoItemPreco.Adicional, 5)
                            .MontarESalvarItemAdicionais(this.MassaBuilder.ItensPrecos.Last().Id, 5)
                            .MontarPedido(this.MassaBuilder.Pedido, this.MassaBuilder.ItensPrecos, this.MassaBuilder.ItensAdicionais);
        }

        [When(@"sistema salva a personalizacao do pedido")]
        public void WhenSistemaSalvaAPersonalizacaoDoPedido()
        {
            var service = new PedidoService(new PedidoRepository(), new ModoPreparoRepository(), 
                                            new ItemPrecoRepository(), new ItemAdicionalRepository(),
                                            new SaborRepository());
            
            var controller = new PedidoController(service);
            this.Result = controller.Put(this.MassaBuilder.Pedido); 

            var createdResponse = this.Result as OkObjectResult;
            this.MassaBuilder.Pedido = createdResponse.Value as Pedido;
        }

        [Then(@"o pedido é salvo com todas as personalizações escolhidas bem como adicional de preço e tempo de preparo")]
        public void ThenOpedidoEsalvoComTodasAsPersonalizacoesEscolhidasBemComoAdicionalDePrecoEdePreparo()
        {
            var itensAdicionais = new List<ItemAdicional>();
            var itensPrecos = new List<ItemPreco>();
            
            foreach(var item in this.MassaBuilder.ItensAdicionais)
            {
                var adicional = this.MassaBuilder.ConsultarItemAdicional(item.Id);
                itensAdicionais.Add(adicional);

                var itemPreco = this.MassaBuilder.ConsultarItemPreco(item.ItemPreco.Id);
                itensPrecos.Add(itemPreco);
            }

            this.MassaBuilder.Pedido.ItensAdicionais = itensAdicionais;

            var totalValor = itensPrecos.Sum(i => i.Valor);
            var totalTempoPreparo = itensAdicionais.Sum(i => i.TempoDePreparo) + this.MassaBuilder.ModoPreparo.TempoDePreparo;
            bool contemTempoDePreparo = totalTempoPreparo > 0;                      
            bool contemPreco = totalValor > 0;
            
            Assert.IsTrue(contemTempoDePreparo);
            Assert.IsTrue(contemPreco);
        }

        [Then(@"os valores e tempos adicionais devem ser somados no total do pedido")]
        public void ThenOsValoresEtemposAdicionaisDevemSerSomadosNoTotalDoPedido()
        {
            decimal totalValor = this.MassaBuilder.ItensPrecos.Sum(p => p.Valor);
            var totalTempoPreparo = this.MassaBuilder.Pedido.ItensAdicionais.Sum(i => i.TempoDePreparo) + this.MassaBuilder.ModoPreparo.TempoDePreparo;

            Assert.AreEqual(this.MassaBuilder.Pedido.Valor, totalValor);
            Assert.AreEqual(this.MassaBuilder.Pedido.TempoPreparo, totalTempoPreparo);

            Destruir();
        }

        [Then(@"o pedido é salvo sem adicionais")]
        public void ThenOpedidoEsalvoSemAdicionais()
        {
            var createdResponse = this.Result as CreatedResult;
            this.MassaBuilder.Pedido = createdResponse.Value as Pedido;

            bool semAdicionais = this.MassaBuilder.Pedido.ItensAdicionais == null;
            
            Assert.IsTrue(semAdicionais);
        }

        [When(@"sistema salva o pedido")]
        public void ThenOsistemaSalvaOpedido()
        {
            var service = new PedidoService(new PedidoRepository(), new ModoPreparoRepository(), 
                                            new ItemPrecoRepository(), new ItemAdicionalRepository(),
                                            new SaborRepository());
            
            var controller = new PedidoController(service);
            this.Result = controller.Post(this.MassaBuilder.Pedido); 
        }        

        [Then(@"com o mesmo valor e tempo de preparo da montagem da pizza")]
        public void ThenComOmesmoValorEtempoDePreparoDaMontagemDaPizza()
        {
            var modoPreparo = this.MassaBuilder.ConsultarModoPreparo(this.MassaBuilder.Pedido.ModoPreparo.Id);
            var itemPreco = this.MassaBuilder.ConsultarItemPreco(modoPreparo.ItemPreco.Id);

            bool mesmoValor = this.MassaBuilder.Pedido.Valor == itemPreco.Valor;
            bool mesmoTempoDePreparo = this.MassaBuilder.ModoPreparo.TempoDePreparo == modoPreparo.TempoDePreparo;
            
            Assert.IsTrue(mesmoValor);
            Assert.IsTrue(mesmoTempoDePreparo);

            Destruir();
        }
    }
}