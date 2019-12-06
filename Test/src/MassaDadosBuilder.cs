using System;
using System.Collections.Generic;
using System.Linq;
using Webapi.Entities;
using Webapi.Enums;

namespace Test.src
{
    public class MassaDadosBuilder
    {
        public Pedido Pedido {get; set;}
        public Sabor Sabor {get; set; }
        public ModoPreparo ModoPreparo {get; set;}
        public List<ItemAdicional> ItensAdicionais {get; set;}
        public List<ItemPreco> ItensPrecos { get; set; }
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
        public MassaDadosBuilder MontarPedido(int modoPreparoId, int tempoPreparo, decimal valor)
        {
            this.Pedido = new Pedido()
            {
                ModoPreparo = new ModoPreparo(){ Id = modoPreparoId},
                Valor = valor,
                Quantidade = 1,
                Data = DateTime.Now,
                TempoPreparo = tempoPreparo,
                Status = (int)StatusPedido.Andamento
            };
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
        public MassaDadosBuilder MontarPedido(int modoPreparoId, List<ItemAdicional> itensAdicionais, int tempoPreparo, decimal valorPizza)
        {
            MontarPedido(modoPreparoId, tempoPreparo, valorPizza);

            List<ItemAdicional> itensAdicionaisLista = new List<ItemAdicional>();

            for(int i= 0; i < itensAdicionais.Count; i++){
                itensAdicionaisLista.Add(new ItemAdicional(){ Id = itensAdicionais.ElementAt(i).Id});
            }
            this.Pedido.ItensAdicionais = itensAdicionaisLista;
            this.Pedido.Valor = this.ItensPrecos.Select(i => i.Valor).Sum();
            this.Pedido.TempoPreparo += itensAdicionaisLista.Sum(i => i.TempoDePreparo);

            return this;
        }
        public MassaDadosBuilder MontarModoPreparo(int itemPrecoId, int tamanho, int saborId )
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

            return this;
        }
        public MassaDadosBuilder MontarSabor(string nome)
        {
            this.Sabor = new Sabor()
            {
                Nome = nome
            };
            return this;
        }
        public MassaDadosBuilder MontarItemPreco(int tipo, decimal valor)
        {
            this.ItensPrecos = this.ItensPrecos == null ? new List<ItemPreco>() : this.ItensPrecos;
            var item = new ItemPreco()
                                {
                                    DataCadastro = DateTime.Now,
                                    Tipo = tipo,
                                    Valor = valor
                                };
            this.ItensPrecos.Add(item);
            return this;
        }
        public MassaDadosBuilder MontarItemAdicionais(int itemPrecoId)
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
                    TempoDePreparo = 5
                }
            };
            return this;
        }
    }
}