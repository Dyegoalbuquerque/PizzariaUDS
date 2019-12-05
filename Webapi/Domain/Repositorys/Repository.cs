using Webapi.Data;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys
{
    public class Repository<T> where T : Entity
    {
        protected Dao<T> Dao { get; set; }
        public Repository()
        {
            this.Dao = new Dao<T>();
        }
    }
}