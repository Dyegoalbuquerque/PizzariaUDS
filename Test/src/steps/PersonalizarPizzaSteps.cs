using TechTalk.SpecFlow;
using System;
using NUnit.Framework;
using Webapi.Domain.Repositorys.Abstract;
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
        private IPedidoRepository PedidoRepository { get; set; }
        private IModoPreparoRepository ModoPreparoRepository { get; set; }
        private IItemPrecoRepository ItemPrecoRepository { get; set; }
      
        private ISaborRepository SaborRepository { get; set; }
        private IActionResult Result { get; set; }
        private IItemAdicionalRepository ItemAdicionalRepository { get; set; }

        private void Inicializar()
        {
            this.MassaBuilder = new MassaDadosBuilder();
            this.PedidoRepository = new PedidoRepository();
            this.ModoPreparoRepository = new ModoPreparoRepository();
            this.ItemPrecoRepository = new ItemPrecoRepository();
            this.SaborRepository = new SaborRepository();
            this.ItemAdicionalRepository = new ItemAdicionalRepository();
         }

        private void Destruir()
        {
            foreach(var item in this.MassaBuilder.Pedido.ItensAdicionais)
            {
                this.ItemAdicionalRepository.RemoverPorId(item.Id);
            }
            this.PedidoRepository.RemoverPorId(this.MassaBuilder.Pedido.Id);
            this.ModoPreparoRepository.RemoverPorId(this.MassaBuilder.ModoPreparo.Id);
            this.SaborRepository.RemoverPorId(this.MassaBuilder.Sabor.Id);
            foreach(var item in this.MassaBuilder.ItensPrecos)
            {
                this.ItemPrecoRepository.RemoverPorId(item.Id);
            }
        }

        [Given(@"um cliente que montou sua pizza com tamanho e sabor")]
        public void GivenUmClienteQueMontouSuaPizzaComTamanhoESabor()
        {
            Inicializar();

           this.MassaBuilder.MontarItemPreco((int)TipoItemPreco.ModoPreparo, 40);
           var itemPreco = this.MassaBuilder.ItensPrecos.First();
           var itemPrecoId = this.ItemPrecoRepository.Adicionar(itemPreco);
           itemPreco.Id = itemPrecoId;

           this.MassaBuilder.MontarSabor("Baiana");
           var saborId = this.SaborRepository.Adicionar(this.MassaBuilder.Sabor);
           this.MassaBuilder.Sabor.Id = saborId;

           this.MassaBuilder.MontarModoPreparo(itemPrecoId, (int)TamanhoPizza.Grande, saborId); 
           var modoPreparoId = this.ModoPreparoRepository.Adicionar(this.MassaBuilder.ModoPreparo);
           this.MassaBuilder.ModoPreparo.Id = modoPreparoId;  

           this.MassaBuilder.MontarPedido(this.MassaBuilder.ModoPreparo.Id, this.MassaBuilder.ModoPreparo.TempoDePreparo, itemPreco.Valor);           
        }

        [Given(@"que personalizou seu pedido adicionando itens tornando única pizza")]
        public void GivenQuePersonalizouSeuPedidoAdicionandoItensTornandoUnicaPizza()
        {                     
           var pedidoId = this.PedidoRepository.Adicionar(this.MassaBuilder.Pedido);
           this.MassaBuilder.Pedido.Id = pedidoId;

           this.MassaBuilder.MontarItemPreco((int)TipoItemPreco.Adicional, 5);
           var itemPreco = this.MassaBuilder.ItensPrecos.Last();
           itemPreco.Id = this.ItemPrecoRepository.Adicionar(itemPreco);

           this.MassaBuilder.MontarItemAdicionais(itemPreco.Id);
           foreach(var item in this.MassaBuilder.ItensAdicionais)
           {              
                this.ItemAdicionalRepository.Adicionar(item);
           }

           this.MassaBuilder.MontarPedido(this.MassaBuilder.Pedido, this.MassaBuilder.ItensPrecos, this.MassaBuilder.ItensAdicionais);
        }

        [When(@"sistema salva a personalizacao do pedido")]
        public void WhenSistemaSalvaAPersonalizacaoDoPedido()
        {
            var service = new PedidoService(this.PedidoRepository, this.ModoPreparoRepository, this.ItemPrecoRepository, this.ItemAdicionalRepository);
            
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
                var adicional = this.ItemAdicionalRepository.BuscarPorId(item.Id);
                itensAdicionais.Add(adicional);

                var itemPreco = this.ItemPrecoRepository.BuscarPorId(item.ItemPreco.Id);
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

        [Then(@"sistema salva o pedido")]
        public void ThenOsistemaSalvaOpedido()
        {
            var service = new PedidoService(this.PedidoRepository, this.ModoPreparoRepository, this.ItemPrecoRepository, this.ItemAdicionalRepository);
            
            var controller = new PedidoController(service);
            this.Result = controller.Post(this.MassaBuilder.Pedido); 
        }        

        [Then(@"com o mesmo valor e tempo de preparo da montagem da pizza")]
        public void ThenComOmesmoValorEtempoDePreparoDaMontagemDaPizza()
        {
            var modoPreparo = this.ModoPreparoRepository.BuscarPorId(this.MassaBuilder.Pedido.ModoPreparo.Id);
            var itemPreco = this.ItemPrecoRepository.BuscarPorId(modoPreparo.ItemPreco.Id);

            bool mesmoValor = this.MassaBuilder.Pedido.Valor == itemPreco.Valor;
            bool mesmoTempoDePreparo = this.MassaBuilder.ModoPreparo.TempoDePreparo == modoPreparo.TempoDePreparo;
            
            Assert.IsTrue(mesmoValor);
            Assert.IsTrue(mesmoTempoDePreparo);

            Destruir();
        }
    }
}