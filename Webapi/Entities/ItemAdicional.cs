namespace Webapi.Entities
{
    public class ItemAdicional : Entity
    {
        public string Nome { get; set; }
        public ItemPreco ItemPreco{ get; set; }
        public int TempoDePreparo { get; set; }
        public string UrlImagem {get; set;}
    }
}