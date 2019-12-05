using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Abstract
{
    public interface IModoPreparoRepository
    {
              void RemoverPorId(int id);
           ModoPreparo BuscarPorId(int id);
           int Adicionar(ModoPreparo item);
           List<ModoPreparo> ObterTodos();
    }
}