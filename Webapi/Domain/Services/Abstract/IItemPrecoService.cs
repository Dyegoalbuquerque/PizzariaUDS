using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Services.Abstract
{
    public interface IItemPrecoService
    {
         List<ItemPreco> ObterTodos();
    }
}