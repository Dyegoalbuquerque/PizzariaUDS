using System;
using System.Collections.Generic;
using System.Linq;
using Webapi.Domain.Repositorys.Abstract;
using Webapi.Domain.Repositorys.Concrete;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;
using Webapi.Enums;
using Webapi.Exceptions;

namespace Webapi.Domain.Services.Concrete
{
    public class PedidoService : IPedidoService
    {
        private IPedidoRepository Repository { get; set;}
        private IModoPreparoRepository ModoPreparoRepository {get; set;}

        private IItemPrecoRepository ItemPrecoRepository {get; set;}

        private IItemAdicionalRepository ItemAdicionalRepository {get; set;}
        public PedidoService(IPedidoRepository repository, IModoPreparoRepository modoPreparoRepository, 
                             IItemPrecoRepository itemPrecoRepository, IItemAdicionalRepository itemAdicionalRepository)
        {
            this.Repository = repository;
            this.ModoPreparoRepository = modoPreparoRepository;
            this.ItemPrecoRepository = itemPrecoRepository;
            this.ItemAdicionalRepository = itemAdicionalRepository;
        }

        public int Adicionar(Pedido item)
        {
           return this.Repository.Adicionar(item);
        }
        public List<Pedido> ObterTodos()
        {
            return this.Repository.ObterPedidos();
        }

        public void RemoverTodos()
        {
            this.Repository.RemoverTodos();
        }

        public int Atualizar(Pedido item)
        {
            return this.Repository.Atualizar(item);
        }

        public Pedido BuscarPorId(int id)
        {
            return this.Repository.BuscarPorId(id);
        }

        public Pedido MontarPizza(Pedido item)
        {       
            if(item.Id == 0)
            {
                var modoPreparo = this.ModoPreparoRepository.BuscarPorId(item.ModoPreparo.Id);
                var itemPreco = this.ItemPrecoRepository.BuscarPorId(modoPreparo.ItemPreco.Id);

                item.Data = DateTime.Now;
                item.Status = (int)StatusPedido.Andamento;
                item.Quantidade = 1;
                item.Valor = itemPreco.Valor;
                item.TempoPreparo = modoPreparo.TempoDePreparo;
                item.Id = Adicionar(item);

            }else {
                throw new PedidoException("Não pode criar um pedido já existente");
            }

            return item;
        }

        public Pedido PersonalizarPizza(Pedido item)
        {
            if(item.Id > 0)
            {
                var pedido = this.Repository.BuscarPorId(item.Id);
                
                var modoPreparo = this.ModoPreparoRepository.BuscarPorId(item.ModoPreparo.Id);
                
                var itemAdicionalIds = item.ItensAdicionais.Select(i => i.Id).ToList();
                var itensAdicionais = this.ItemAdicionalRepository.BuscarPorIds(itemAdicionalIds);
                
                var itemPrecoIds = itensAdicionais.Select(i => i.ItemPreco).Select(i => i.Id).ToList();
                var itensPrecos = this.ItemPrecoRepository.BuscarPorIds(itemPrecoIds);
                
                pedido.Valor += itensPrecos.Sum(i => i.Valor);
                pedido.TempoPreparo += itensAdicionais.Sum(i => i.TempoDePreparo);

                Atualizar(pedido);

                item = pedido;

            }else {
                throw new PedidoException("Pedido não existente");
            }

            return item;
        }

        public Pedido MontarDetalhesPedido(int id)
        {
            var pedido = this.Repository.BuscarPorId(id);
            var adicionais = this.ItemAdicionalRepository.BuscarPorIds(pedido.ItensAdicionais.Select(i => i.Id));
           
            foreach(var adicional in adicionais)
            {
               adicional.ItemPreco = this.ItemPrecoRepository.BuscarPorId(adicional.ItemPreco.Id);
            }
            pedido.ItensAdicionais = adicionais;
            pedido.ModoPreparo = this.ModoPreparoRepository.BuscarPorId(pedido.ModoPreparo.Id);
           
            return pedido;
        }
    }
}