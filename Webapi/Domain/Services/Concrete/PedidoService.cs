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

        private ISaborRepository SaborRepository {get; set;}

        private IItemAdicionalRepository ItemAdicionalRepository {get; set;}
        public PedidoService(IPedidoRepository repository, IModoPreparoRepository modoPreparoRepository, 
                             IItemPrecoRepository itemPrecoRepository, IItemAdicionalRepository itemAdicionalRepository,
                             ISaborRepository saborRepository)
        {
            this.Repository = repository;
            this.ModoPreparoRepository = modoPreparoRepository;
            this.ItemPrecoRepository = itemPrecoRepository;
            this.ItemAdicionalRepository = itemAdicionalRepository;
            this.SaborRepository = saborRepository;
        }

        public int Adicionar(Pedido item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Argumento item não pode ser nulo");
            }
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
            if (item == null)
            {
                throw new ArgumentNullException("Argumento item não pode ser nulo");
            }
            return this.Repository.Atualizar(item);
        }

        public Pedido BuscarPorId(int id)
        {
            return this.Repository.BuscarPorId(id);
        }
     
        public Pedido MontarPizza(Pedido item)
        {    
            if (item == null)
            {
                throw new ArgumentNullException("Argumento item não pode ser nulo");
            }

            if(item.Id == 0)
            {
                int tamanho = item.ModoPreparo.Tamanho;
                int saborId = item.ModoPreparo.Sabor.Id;
                var modoPreparo = this.ModoPreparoRepository.BuscarPorTamanhoESabor(tamanho, saborId);
                var itemPreco = this.ItemPrecoRepository.BuscarPorId(modoPreparo.ItemPreco.Id);

                item.ModoPreparo = new ModoPreparo(){Id = modoPreparo.Id,};
                item.Data = DateTime.Now;
                item.Status = (int)StatusPedido.Andamento;
                item.Quantidade = 1;
                item.CalcularValor(itemPreco);
                item.CalcularTempoDePreparo(modoPreparo);
                item.Id = Adicionar(item);

                item.ModoPreparo.Tamanho = tamanho;
                item.ModoPreparo.Sabor = new Sabor(){Id = saborId};

            }else {
                throw new PedidoException("Não pode criar um pedido já existente");
            }

            return item;
        }
        public Pedido PersonalizarPizza(Pedido item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Argumento item não pode ser nulo");
            }

            if(item.Id > 0)
            {
                var pedido = this.Repository.BuscarPorId(item.Id);
                
                var modoPreparo = this.ModoPreparoRepository.BuscarPorId(item.ModoPreparo.Id);
                
                var itemAdicionalIds = item.ItensAdicionais.Select(i => i.Id).ToList();
                var itensAdicionais = this.ItemAdicionalRepository.BuscarPorIds(itemAdicionalIds);
                
                var itemPrecoIds = itensAdicionais.Select(i => i.ItemPreco).Select(i => i.Id).ToList();
                var itensPrecosTotais = this.ItemPrecoRepository.BuscarPorIds(itemPrecoIds);
                var itemPrecoDaPizza = this.ItemPrecoRepository.BuscarPorId(modoPreparo.ItemPreco.Id);
                var sabor = this.SaborRepository.BuscarPorId(modoPreparo.Sabor.Id);
                
                foreach(var i in itensAdicionais)
                {
                    i.ItemPreco = itensPrecosTotais.Single(ia => ia.Id == i.ItemPreco.Id);
                }

                pedido.Valor += itensPrecosTotais.Sum(i => i.Valor);
                pedido.TempoPreparo += itensAdicionais.Sum(i => i.TempoDePreparo);
                pedido.ItensAdicionais = item.ItensAdicionais;
                pedido.ModoPreparo = modoPreparo;
                pedido.ModoPreparo.Sabor = sabor;
                pedido.ModoPreparo.ItemPreco = itemPrecoDaPizza;
                pedido.ItensAdicionais = itensAdicionais;

                Atualizar(pedido);

                item = pedido;

            }else {
                throw new PedidoException("Pedido não existente");
            }

            return item;
        }
        public Pedido MontarDetalhesPedido(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException("Argumento id não pode ser 0");
            }

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