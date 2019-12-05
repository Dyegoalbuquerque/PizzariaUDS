using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Domain.Repositorys.Concrete;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Services.Concrete
{
    public class SaborService: ISaborService
    {
        private ISaborRepository Repository { get; set;}
        public SaborService(ISaborRepository repository)
        {
            this.Repository = repository;
        }

        public List<Sabor> ObterTodos()
        {
            return this.Repository.ObterTodos();
        }
    }
}