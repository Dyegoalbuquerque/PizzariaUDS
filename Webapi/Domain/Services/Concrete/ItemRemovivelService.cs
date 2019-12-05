using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Services.Concrete
{
    public class ItemRemovivelService : IItemRemovivelService
    {
        private IItemRemovivelRepository ItemRemovivelRepository {get; set; }
        public ItemRemovivelService(IItemRemovivelRepository repository)
        {
            this.ItemRemovivelRepository = repository;
        } 
        public List<ItemRemovivel> ObterTodos()
        {
            return this.ItemRemovivelRepository.ObterTodos();
        }
    }
}