using TechTalk.SpecFlow;
using System;
using NUnit.Framework;
using Test.src;
using Microsoft.AspNetCore.Mvc;
using Webapi.Domain.Repositorys.Concrete;
using Webapi.Enums;
using System.Linq;
using Webapi.Domain.Services.Concrete;
using Webapi.Controllers;
using Webapi.Entities;

namespace Webapi.Test.src.steps
{
    [Binding]
    public class MontarPedidoSteps
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

        [Given(@"um cliente que deseja visualizar os detalhes do pedido montado anteriormente")]
        public void GivenUmClienteQueDesejaVisualizarOsDetalhesDoPedidoMontadoAnteriormente()
        {
            Inicializar();

           this.MassaBuilder.MontarESalvarItemPreco((int)TipoItemPreco.ModoPreparo, 40)
                            .MontarESalvarSabor("Baiana")
                            .MontarESalvarModoPreparo(this.MassaBuilder.ItensPrecos.First().Id, (int)TamanhoPizza.Grande, this.MassaBuilder.Sabor.Id)
                            .MontarESalvarItemPreco((int)TipoItemPreco.Adicional, 5)
                            .MontarESalvarItemAdicionais(this.MassaBuilder.ItensPrecos.Last().Id, 5)
                            .MontarESalvarPedido(this.MassaBuilder.ItensAdicionais, this.MassaBuilder.ModoPreparo.ItemPreco.Valor, 
                                                 this.MassaBuilder.Sabor.Id, this.MassaBuilder.ModoPreparo);  
        }

        [When(@"sistema consulta o pedido")]
        public void WhenSistemaConsultaOPedido()
        {
            var service = new PedidoService(new PedidoRepository(), new ModoPreparoRepository(), 
                                            new ItemPrecoRepository(), new ItemAdicionalRepository(),
                                            new SaborRepository());
            
            var controller = new PedidoController(service);
            this.Result = controller.Get(this.MassaBuilder.Pedido.Id); 

            var createdResponse = this.Result as OkObjectResult;
            this.MassaBuilder.Pedido = createdResponse.Value as Pedido;
        }

        [Then(@"o sistema apresenta o resumo do pedido listando sabor da pizza escolhida juntamente com o preço")]
        public void ThenOsistemaApresentaOresumoDoPedidoListandoSaborDaPizzaEscolhidaJuntamenteComOpreco()
        {
           bool temSabor = this.MassaBuilder.Pedido.ModoPreparo.Sabor != null;
           bool temPreco = this.MassaBuilder.Pedido.Valor > 0;

           Assert.IsTrue(temSabor);
           Assert.IsTrue(temPreco);
        }

        [Then(@"a lista de personalização que foi selecionado e seu valor")]
        public void ThenAlistaDePersonalizacaoQueFoiSelecionadoEseuValor()
        {
           var adicionais = this.MassaBuilder.Pedido.ItensAdicionais;
           bool temPersonalizacao = adicionais != null && adicionais.Any();
           bool temPersonalizacaoPreco = adicionais.Select(i => i.ItemPreco).Sum(i => i.Valor) > 0;

           Assert.IsTrue(temPersonalizacao);
           Assert.IsTrue(temPersonalizacaoPreco);
        }

        [Then(@"o valor total do pedido e seu tempo de preparo")]
        public void ThenOvalorTotalDoPedidoEseuTempoDePreparo()
        {
           bool temValorTotal = this.MassaBuilder.Pedido.Valor > 0;
           bool temTempoDePreparo = this.MassaBuilder.Pedido.TempoPreparo > 0;

           Assert.IsTrue(temValorTotal);
           Assert.IsTrue(temTempoDePreparo);

           Destruir();
        }
    }
}