using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Concrete
{
    public class ModoPreparoRepository : Repository<ModoPreparo>, IModoPreparoRepository
    {
         public void RemoverPorId(int id)
        {
            this.Dao.RemoverPorId(id);
        }
        public int Adicionar(ModoPreparo item)
        {
            return this.Dao.Adicionar(item);
        }

        public ModoPreparo BuscarPorId(int id)
        {
            return this.Dao.BuscarPorId(id);
        }

        public List<ModoPreparo> ObterTodos()
        {
            return this.Dao.ObterTodos();
        }
    }
}