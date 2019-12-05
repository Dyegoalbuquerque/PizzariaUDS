using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Services.Concrete
{
    public class ItemPrecoService : IItemPrecoService
    {
        private IItemPrecoRepository Repository { get; set;}
        public ItemPrecoService(IItemPrecoRepository repository)
        {
            this.Repository = repository;
        }
        public List<ItemPreco> ObterTodos()
        {
            return this.Repository.ObterTodos();
        }
    }
}