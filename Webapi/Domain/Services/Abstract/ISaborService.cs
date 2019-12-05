using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Services.Abstract
{
    public interface ISaborService
    {
           List<Sabor> ObterTodos();
    }
}