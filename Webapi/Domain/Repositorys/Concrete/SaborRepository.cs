using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Concrete
{
    public class SaborRepository : Repository<Sabor>, ISaborRepository
    {
        public int Adicionar(Sabor item)
        {
            return this.Dao.Adicionar(item);
        }

        public List<Sabor> ObterTodos()
        {
            return this.Dao.ObterTodos();
        }
         public void RemoverPorId(int id)
        {
            this.Dao.RemoverPorId(id);
        }
    }
}