using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Domain.Repositorys.Concrete;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Services.Concrete
{
    public class ModoPreparoService : IModoPreparoService
    {
        private IModoPreparoRepository Repository { get; set;}
        public ModoPreparoService(IModoPreparoRepository repository)
        {
            this.Repository = repository;
        }
        public List<ModoPreparo> ObterTodos()
        {
            return this.Repository.ObterTodos();
        }
    }
}