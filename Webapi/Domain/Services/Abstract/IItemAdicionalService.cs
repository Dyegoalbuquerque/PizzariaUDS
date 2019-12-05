using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Services.Abstract
{
    public interface IItemAdicionalService
    {
         List<ItemAdicional> ObterTodos();
    }
}