using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Concrete
{
    public class ItemRemovivelRepository :  Repository<ItemRemovivel>, IItemRemovivelRepository
    {
        public List<ItemRemovivel> ObterTodos()
        {
             return this.Dao.ObterTodos();
        }
    }
}