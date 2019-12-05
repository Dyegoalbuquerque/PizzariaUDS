using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Abstract
{
    public interface IItemRemovivelRepository
    {
          List<ItemRemovivel> ObterTodos();
    }
}