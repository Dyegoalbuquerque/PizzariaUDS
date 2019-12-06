using System;
using System.Collections.Generic;

namespace Webapi.Entities
{
    public class Pedido : Entity
    {
        public int Status {get; set;}
        public decimal Valor { get; set; }
        public int TempoPreparo { get; set; }   
        public DateTime Data { get; set; }
        public ModoPreparo ModoPreparo { get; set; }
        public int Quantidade {get; set; }
        public List<ItemAdicional> ItensAdicionais { get; set; }

        public void CalcularTempoDePreparo(ModoPreparo item){
            this.TempoPreparo = item == null ? this.TempoPreparo : item.TempoDePreparo;
        }
        public void CalcularValor(ItemPreco item){
            this.Valor = item == null ? this.Valor : item.Valor;
        }
    }
}