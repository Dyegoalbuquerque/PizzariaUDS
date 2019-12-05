using Webapi.Domain.Repositorys;
using Webapi.Entities;

namespace Webapi.Domain.Services
{
    public class Service<T> where T : Entity
    {
        protected Repository<T> Repository{ get; set; }
    }
}