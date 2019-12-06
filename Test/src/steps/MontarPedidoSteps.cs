using TechTalk.SpecFlow;
using System;
using NUnit.Framework;
using Test.src;
using Webapi.Domain.Repositorys.Abstract;
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

        [Given(@"um cliente que deseja visualizar os detalhes do pedido montado anteriormente")]
        public void GivenUmClienteQueDesejaVisualizarOsDetalhesDoPedidoMontadoAnteriormente()
        {
            Inicializar();

           this.MassaBuilder.MontarItemPreco((int)TipoItemPreco.ModoPreparo, 40);
           var itemPreco = this.MassaBuilder.ItensPrecos.First();
           itemPreco.Id = this.ItemPrecoRepository.Adicionar(itemPreco);

           this.MassaBuilder.MontarSabor("Baiana");
           this.MassaBuilder.Sabor.Id = this.SaborRepository.Adicionar(this.MassaBuilder.Sabor);

           this.MassaBuilder.MontarModoPreparo(itemPreco.Id, (int)TamanhoPizza.Grande, this.MassaBuilder.Sabor.Id); 
           var modoPreparoId = this.ModoPreparoRepository.Adicionar(this.MassaBuilder.ModoPreparo);
           this.MassaBuilder.ModoPreparo.Id = modoPreparoId;  


           this.MassaBuilder.MontarItemPreco((int)TipoItemPreco.Adicional, 5);
           itemPreco = this.MassaBuilder.ItensPrecos.Last();
           itemPreco.Id = this.ItemPrecoRepository.Adicionar(itemPreco);

           this.MassaBuilder.MontarItemAdicionais(itemPreco.Id);
           foreach(var item in this.MassaBuilder.ItensAdicionais)
           {              
                this.ItemAdicionalRepository.Adicionar(item);
           }
           
           this.MassaBuilder.MontarPedido(this.MassaBuilder.ModoPreparo.Id, this.MassaBuilder.ItensAdicionais, this.MassaBuilder.ModoPreparo.TempoDePreparo, this.MassaBuilder.ModoPreparo.ItemPreco.Valor);          
           this.MassaBuilder.Pedido.Id = this.PedidoRepository.Adicionar(this.MassaBuilder.Pedido);
        }

        [When(@"sistema consulta o pedido")]
        public void WhenSistemaConsultaOPedido()
        {
            var service = new PedidoService(this.PedidoRepository, this.ModoPreparoRepository, this.ItemPrecoRepository, this.ItemAdicionalRepository);
            
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