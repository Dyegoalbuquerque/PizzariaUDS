using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Concrete
{
    public class ItemAdicionalRepository : Repository<ItemAdicional>, IItemAdicionalRepository
    {
        public int Adicionar(ItemAdicional item)
        {
            return this.Dao.Adicionar(item);
        }

        public ItemAdicional BuscarPorId(int id)
        {
            return this.Dao.BuscarPorId(id);
        }

        public List<ItemAdicional> BuscarPorIds(IEnumerable<int> ids)
        {
           return this.Dao.BuscarPorIds(ids);
        }

        public List<ItemAdicional> ObterTodos()
        {
            return this.Dao.ObterTodos();
        }
        public void RemoverPorId(int id)
        {
            this.Dao.RemoverPorId(id);
        }
    }
}