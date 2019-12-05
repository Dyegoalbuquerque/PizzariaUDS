using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Repositorys.Abstract
{
    public interface IPedidoRepository
    {
        void RemoverPorId(int id);
        Pedido BuscarPorId(int id);
        int Atualizar(Pedido item);
        int Adicionar(Pedido item);
        List<Pedido> ObterPedidos();
        void RemoverTodos();
    }
}