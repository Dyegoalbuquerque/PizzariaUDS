using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Concrete
{
    public class ItemPrecoRepository : Repository<ItemPreco>, IItemPrecoRepository
    {
        public int Adicionar(ItemPreco item)
        {
            return this.Dao.Adicionar(item);
        }

        public ItemPreco BuscarPorId(int id)
        {
            return this.Dao.BuscarPorId(id);
        }

        public List<ItemPreco> BuscarPorIds(IEnumerable<int> ids)
        {
            return this.Dao.BuscarPorIds(ids);
        }

        public List<ItemPreco> ObterTodos()
        {
             return this.Dao.ObterTodos();
        }

        public void RemoverPorId(int id)
        {
            this.Dao.RemoverPorId(id);
        }
    }
}