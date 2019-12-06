using System.Collections.Generic;

namespace Webapi.Entities
{
    public class Sabor : Entity
    {
        public string Nome { get; set; }
        public string UrlImagem {get; set;}
         public string Observacao {get; set;}
    }
}