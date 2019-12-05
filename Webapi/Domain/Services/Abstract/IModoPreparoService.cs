using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Services.Abstract
{
    public interface IModoPreparoService
    {
           List<ModoPreparo> ObterTodos();
    }
}