using TechTalk.SpecFlow;
using NUnit.Framework;
using Webapi.Entities;
using Webapi.Controllers;
using Webapi.Domain.Services.Concrete;
using Webapi.Domain.Repositorys.Concrete;
using Microsoft.AspNetCore.Mvc;
using Webapi.Enums;
using System.Linq;

namespace Test.src.steps
{
    [Binding]
    public class MontarPizzaSteps
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

        [Given(@"um cliente que escolheu tamanho e sabor da sua pizza")]
        public void GivenUmClienteQueEscolheuTamanhoESaborDaSuaPizza()
        {
           Inicializar();

           this.MassaBuilder.MontarESalvarItemPreco((int)TipoItemPreco.ModoPreparo, 40)          
                            .MontarESalvarSabor("Baiana")
                            .MontarESalvarModoPreparo(this.MassaBuilder.ItensPrecos.First().Id, (int)TamanhoPizza.Grande, this.MassaBuilder.Sabor.Id)
                            .MontarPedido(this.MassaBuilder.ModoPreparo.Id, this.MassaBuilder.Sabor.Id, (int)TamanhoPizza.Grande);                 
        }

        [When(@"sistema monta a pizza")]
        public void WhenSistemaMontaApizza()
        {
            var service = new PedidoService(new PedidoRepository(), new ModoPreparoRepository(), 
                                            new ItemPrecoRepository(), new ItemAdicionalRepository(),
                                            new SaborRepository());
            
            var controller = new PedidoController(service);
            this.Result = controller.Post(this.MassaBuilder.Pedido); 

            var createdResponse = this.Result as CreatedResult;
            this.MassaBuilder.Pedido = createdResponse.Value as Pedido;         
        }

        [Then(@"o sistema deve armazenar o pedido com o tempo de preparo bem como o valor final do pedido e os detalhes do produto")]
        public void ThenOsistemaDeveArmazenarOpedidoComOtempoDePreparoBemComoOvalorFinalDoPedidoEosDetalhesDoProduto()
        {            
            bool contemTempoDePreparo = this.MassaBuilder.Pedido.TempoPreparo > 0;
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