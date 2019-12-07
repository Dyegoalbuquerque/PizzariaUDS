using System;
using System.Collections.Generic;
using System.Linq;
using Webapi.Data;
using Webapi.Entities;
using Webapi.Enums;

namespace Test.src
{
    public class MassaDadosBuilder
    {
        private Dao<Pedido> PedidoDao{ get; set; }
        private Dao<ModoPreparo> ModoPreparoDao{ get; set; }
        private Dao<ItemPreco> ItemPrecoDao{ get; set; }
        private Dao<Sabor> SaborDao{ get; set; }
        private Dao<ItemAdicional> ItemAdicionalDao{ get; set; }       
        public Pedido Pedido {get; set;}
        public Sabor Sabor {get; set; }
        public ModoPreparo ModoPreparo {get; set;}
        public List<ItemAdicional> ItensAdicionais {get; set;}
        public List<ItemPreco> ItensPrecos { get; set; }
        private List<object> ObjectCenarios {get; set;}

        public void Inicializar()
        {
            this.ObjectCenarios = new List<object>();
            this.PedidoDao = new Dao<Pedido>();
            this.ModoPreparoDao = new Dao<ModoPreparo>();
            this.ItemPrecoDao = new Dao<ItemPreco>();
            this.SaborDao = new Dao<Sabor>();
            this.ItemAdicionalDao = new Dao<ItemAdicional>();
        }
        public void Destruir()
        {
            if(this.Pedido.ItensAdicionais != null){
                foreach(var item in this.Pedido.ItensAdicionais)
                {
                    this.ItemAdicionalDao.RemoverPorId(item.Id);
                }
            }
            this.PedidoDao.RemoverPorId(this.Pedido.Id);
            this.ModoPreparoDao.RemoverPorId(this.ModoPreparo.Id);
            this.SaborDao.RemoverPorId(this.Sabor.Id);
            foreach(var item in this.ItensPrecos)
            {
                this.ItemPrecoDao.RemoverPorId(item.Id);
            }
        }
        public MassaDadosBuilder MontarPedido(int modoPreparoId, int saborId, int tamanho)
        {
            this.Pedido = new Pedido()
            {
                ModoPreparo = new ModoPreparo()
                { 
                    Id = modoPreparoId, 
                    Sabor = new Sabor(){ Id = saborId },
                    Tamanho = tamanho
                }
            };

            return this;
        }  
        public MassaDadosBuilder MontarESalvarPedido(int modoPreparoId, int saborId, int tamanho)
        {
            MontarPedido(modoPreparoId, saborId, tamanho);

            this.Pedido.Id = this.PedidoDao.Adicionar(this.Pedido);

            this.ObjectCenarios.Add(this.Pedido);

            return this;
        }
        public MassaDadosBuilder MontarPedido(decimal valor, int saborId, ModoPreparo modoPreparo)
        {
            this.Pedido = new Pedido()
            {
                ModoPreparo = new ModoPreparo(){ Id = modoPreparo.Id, Tamanho = modoPreparo.Tamanho, Sabor = new Sabor(){ Id = saborId}},
                Valor = valor,
                Quantidade = 1,
                Data = DateTime.Now,
                TempoPreparo = modoPreparo.TempoDePreparo,
                Status = (int)StatusPedido.Andamento
            };          

            return this;
        } 
        public MassaDadosBuilder MontarESalvarPedido(decimal valor, int saborId, ModoPreparo modoPreparo)
        {
            MontarPedido(valor, saborId, modoPreparo);

            this.Pedido.Id = this.PedidoDao.Adicionar(this.Pedido);

            this.ObjectCenarios.Add(this.Pedido);
               return this;
        }
        public MassaDadosBuilder MontarPedido(Pedido pedido, List<ItemPreco> itensPrecos, List<ItemAdicional> itensAdicionais)
        {
            List<ItemAdicional> itensAdicionaisLista = new List<ItemAdicional>();

            for(int i= 0; i < itensAdicionais.Count; i++){
                itensAdicionaisLista.Add(new ItemAdicional(){ Id = itensAdicionais.ElementAt(i).Id});
            }
            this.Pedido = pedido;
            this.Pedido.ItensAdicionais = itensAdicionaisLista;
            this.Pedido.Valor = itensPrecos.Select(i => i.Valor).Sum();

            return this;
        }
        public MassaDadosBuilder MontarESalvarPedido(Pedido pedido, List<ItemPreco> itensPrecos, List<ItemAdicional> itensAdicionais)
        {
            MontarPedido(pedido, itensPrecos, itensAdicionais);

            this.Pedido.Id = this.PedidoDao.Adicionar(this.Pedido);

            this.ObjectCenarios.Add(this.Pedido);

            return this;
        }
        public MassaDadosBuilder MontarPedido(List<ItemAdicional> itensAdicionais, decimal valorPizza, int saborId, ModoPreparo modoPreparo)
        {
            MontarPedido(valorPizza, saborId, modoPreparo);

            List<ItemAdicional> itensAdicionaisLista = new List<ItemAdicional>();

            for(int i= 0; i < itensAdicionais.Count; i++){
                itensAdicionaisLista.Add(new ItemAdicional(){ Id = itensAdicionais.ElementAt(i).Id});
            }
            this.Pedido.ItensAdicionais = itensAdicionaisLista;
            this.Pedido.Valor = this.ItensPrecos.Select(i => i.Valor).Sum();
            this.Pedido.TempoPreparo += itensAdicionaisLista.Sum(i => i.TempoDePreparo);

            this.Pedido.Id = this.PedidoDao.Adicionar(this.Pedido);

            this.ObjectCenarios.Add(this.Pedido);

            return this;
        }
        public MassaDadosBuilder MontarESalvarPedido(List<ItemAdicional> itensAdicionais, decimal valorPizza, int saborId, ModoPreparo modoPreparo)
        {
            MontarPedido(itensAdicionais, valorPizza, saborId, modoPreparo);

            this.Pedido.Id = this.PedidoDao.Adicionar(this.Pedido);

            this.ObjectCenarios.Add(this.Pedido);
            return this;
        }
        public MassaDadosBuilder MontarESalvarModoPreparo(int itemPrecoId, int tamanho, int saborId )
        {
            this.ModoPreparo = new ModoPreparo()
                {
                    Tamanho = tamanho,
                    Sabor = new Sabor()
                    {
                        Id = saborId
                    },
                    ItemPreco = new ItemPreco(){ Id = itemPrecoId },
                    TempoDePreparo = 25
                };
            this.ModoPreparo.Id = this.ModoPreparoDao.Adicionar(this.ModoPreparo);
            this.ObjectCenarios.Add(this.ModoPreparo);

            return this;
        }
        public MassaDadosBuilder MontarESalvarSabor(string nome)
        {
            this.Sabor = new Sabor()
            {
                Nome = nome
            };

           this.Sabor.Id = this.SaborDao.Adicionar(this.Sabor);
           this.ObjectCenarios.Add(this.Sabor);

           return this;
        }
        public MassaDadosBuilder MontarESalvarItemPreco(int tipo, decimal valor)
        {
            this.ItensPrecos = this.ItensPrecos == null ? new List<ItemPreco>() : this.ItensPrecos;
            var item = new ItemPreco()
                                {
                                    DataCadastro = DateTime.Now,
                                    Tipo = tipo,
                                    Valor = valor
                                };
            item.Id = this.ItemPrecoDao.Adicionar(item);
            this.ItensPrecos.Add(item);
            this.ObjectCenarios.Add(item);

            return this;
        }
        public MassaDadosBuilder MontarESalvarItemAdicionais(int itemPrecoId, int tempoPreparo)
        {
            this.ItensAdicionais = new List<ItemAdicional>()
            {
                new ItemAdicional()
                { 
                    Nome = "Borda recheada" , 
                    ItemPreco = new ItemPreco() 
                    {
                        Id = itemPrecoId
                    },
                    TempoDePreparo = tempoPreparo
                }
            };
           foreach(var item in this.ItensAdicionais)
           {              
                item.Id = this.ItemAdicionalDao.Adicionar(item);
                this.ObjectCenarios.Add(item);
           }

           return this;
        }   
        public ModoPreparo ConsultarModoPreparo(int id)
        {
            return this.ModoPreparoDao.BuscarPorId(id);
        }
        public ItemPreco ConsultarItemPreco(int id)
        {
            return this.ItemPrecoDao.BuscarPorId(id);
        }
        public ItemAdicional ConsultarItemAdicional(int id)
        {
            return this.ItemAdicionalDao.BuscarPorId(id);
        }
    }
}