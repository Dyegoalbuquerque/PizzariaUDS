using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Abstract
{
    public interface IItemAdicionalRepository
    {
          int Adicionar(ItemAdicional item);
          ItemAdicional BuscarPorId(int id);
          void RemoverPorId(int id);
          List<ItemAdicional> ObterTodos();
          List<ItemAdicional> BuscarPorIds(IEnumerable<int> ids);
    }
}