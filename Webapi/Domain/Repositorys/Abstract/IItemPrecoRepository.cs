using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Abstract
{
    public interface IItemPrecoRepository
    {
        void RemoverPorId(int id);
        ItemPreco BuscarPorId(int id);
        int Adicionar(ItemPreco item);
         List<ItemPreco> ObterTodos();
         List<ItemPreco> BuscarPorIds(IEnumerable<int> ids);
    }
}