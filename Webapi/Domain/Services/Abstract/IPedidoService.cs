using System;
using System.Collections.Generic;
using Webapi.Entities;

namespace Webapi.Domain.Services.Abstract
{
    public interface IPedidoService
    {
        Pedido MontarPizza(Pedido item);
        Pedido PersonalizarPizza(Pedido item);
        Pedido MontarDetalhesPedido(int id);
        Pedido BuscarPorId(int id);
        int Atualizar(Pedido item);
        int Adicionar(Pedido item);
         List<Pedido> ObterTodos();
         void RemoverTodos();
    }
}