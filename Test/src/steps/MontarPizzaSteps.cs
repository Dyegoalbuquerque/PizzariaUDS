using TechTalk.SpecFlow;
using NUnit.Framework;
using Webapi.Entities;
using Webapi.Controllers;
using Webapi.Domain.Services.Concrete;
using Webapi.Domain.Repositorys.Concrete;
using Microsoft.AspNetCore.Mvc;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Enums;
using System.Linq;

namespace Test.src.steps
{
    [Binding]
    public class MontarPizzaSteps
    {
        private MassaDadosBuilder MassaBuilder { get; set; }
        private IPedidoRepository PedidoRepository { get; set; }
        private IModoPreparoRepository ModoPreparoRepository { get; set; }
        private IItemPrecoRepository ItemPrecoRepository { get; set; }
       
        private IItemAdicionalRepository ItemAdicionalRepository{ get; set; }
      
        private ISaborRepository SaborRepository { get; set; }
        private IActionResult Result { get; set; }
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
            this.PedidoRepository.RemoverPorId(this.MassaBuilder.Pedido.Id);
            this.ModoPreparoRepository.RemoverPorId(this.MassaBuilder.ModoPreparo.Id);
            this.SaborRepository.RemoverPorId(this.MassaBuilder.Sabor.Id);
            foreach(var item in this.MassaBuilder.ItensPrecos)
            {
                this.ItemPrecoRepository.RemoverPorId(item.Id);
            }
        }

        [Given(@"um cliente que escolheu tamanho e sabor da sua pizza")]
        public void GivenUmClienteQueEscolheuTamanhoESaborDaSuaPizza()
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

           this.MassaBuilder.MontarPedido(modoPreparoId, saborId, (int)TamanhoPizza.Grande);                 
        }

        [When(@"sistema monta a pizza")]
        public void WhenSistemaMontaApizza()
        {
            var service = new PedidoService(this.PedidoRepository, this.ModoPreparoRepository, 
                                            this.ItemPrecoRepository, this.ItemAdicionalRepository,
                                            this.SaborRepository);
            
            var controller = new PedidoController(service);
            this.Result = controller.Post(this.MassaBuilder.Pedido); 

            var createdResponse = this.Result as CreatedResult;
            this.MassaBuilder.Pedido = createdResponse.Value as Pedido;         
        }

        [Then(@"o sistema deve armazenar o pedido com o tempo de preparo bem como o valor final do pedido e os detalhes do produto")]
        public void ThenOsistemaDeveArmazenarOpedidoComOtempoDePreparoBemComoOvalorFinalDoPedidoEosDetalhesDoProduto()
        {            
            var modoPreparo = this.ModoPreparoRepository.BuscarPorId(this.MassaBuilder.Pedido.ModoPreparo.Id);

            bool contemTempoDePreparo = modoPreparo.TempoDePreparo > 0;
            bool contemValor = this.MassaBuilder.Pedido.Valor > 0;
            
            Assert.IsNotNull(this.MassaBuilder.Pedido);
            Assert.IsTrue(contemTempoDePreparo);
            Assert.IsTrue(contemValor);
        }

        [Then(@"com a quantidade de uma pizza por pedido")]
        public void ThenComAquantidadeDeUmaPizzaPorPedido()
        {            
            Assert.AreEqual(1, this.MassaBuilder.Pedido.Quantidade);
        }

        [Then(@"sem a possibilidade de meia pizza")]
        public void ThenSemaPossibilidadeDeMeiaPizza()
        {
            bool naoContemMeiaPizza = this.MassaBuilder.Pedido.Quantidade > 0.5;
            Assert.IsTrue(naoContemMeiaPizza);

            Destruir();
        }
    }
}