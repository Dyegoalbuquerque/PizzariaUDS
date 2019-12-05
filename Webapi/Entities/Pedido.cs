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
    }
}