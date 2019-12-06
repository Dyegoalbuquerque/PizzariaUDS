using System.Collections.Generic;
using System.Linq;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Services.Concrete
{
    public class ItemAdicionalService : IItemAdicionalService
    {
        private IItemAdicionalRepository Repository { get; set;}
        private IItemPrecoRepository ItemPrecoRepository { get; set;}
        public ItemAdicionalService(IItemAdicionalRepository repository, IItemPrecoRepository itemPrecoRepository)
        {
            this.Repository = repository;
            this.ItemPrecoRepository = itemPrecoRepository;
        }

        public List<ItemAdicional> ObterTodos()
        {
            var todos = this.Repository.ObterTodos();
            var itemPrecoIds = todos.Select(i => i.ItemPreco).Select(p => p.Id).ToList();
            var itensPrecos = this.ItemPrecoRepository.BuscarPorIds(itemPrecoIds);
            
            foreach(var item in todos)
            {
               item.ItemPreco = itensPrecos.Single(i => item.ItemPreco.Id == i.Id);
            }

            return todos;
        }
    }
}