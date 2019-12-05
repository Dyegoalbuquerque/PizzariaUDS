using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Abstract
{
    public interface ISaborRepository
    {
         void RemoverPorId(int id);
         int Adicionar(Sabor item);
         List<Sabor> ObterTodos();
    }
}