using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Services.Abstract
{
    public interface IItemRemovivelService
    {        
         List<ItemRemovivel> ObterTodos();
    }
}