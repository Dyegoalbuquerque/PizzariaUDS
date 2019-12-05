using System.Collections.Generic;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Concrete
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        public Pedido BuscarPorId(int id)
        {
            return this.Dao.BuscarPorId(id);
        }
        public int Adicionar(Pedido item)
        {
            return this.Dao.Adicionar(item);
        }

        public int Atualizar(Pedido item)
        {
           return this.Dao.Atualizar(item);
        }

        public List<Pedido> ObterPedidos()
        {
            return this.Dao.ObterTodos();
        }

        public void RemoverTodos()
        {
            this.Dao.RemoverTodos();
        }

         public void RemoverPorId(int id)
        {
            this.Dao.RemoverPorId(id);
        }
    }
}