using System.Collections.Generic;

namespace Webapi.Entities
{
    public class ModoPreparo : Entity
    {
        public Sabor Sabor { get; set; } 
        public ItemPreco ItemPreco { get; set; }
        public string Descricao { get; set; }
        public string Tamanho { get; set; }
        public int TempoDePreparo { get; set; }
        public List<ModoPreparoIngrediente> ModoPreparoIngredientes { get; set; }
    }
}