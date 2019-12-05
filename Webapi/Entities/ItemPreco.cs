using System;

namespace Webapi.Entities
{
    public class ItemPreco : Entity
    {
        public DateTime DataCadastro { get; set; }
        public int Tipo { get; set; }
        public decimal Valor { get; set; }
        
    }
}