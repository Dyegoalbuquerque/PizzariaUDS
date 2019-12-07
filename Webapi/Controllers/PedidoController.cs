using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private IPedidoService PedidoService {get; set; }
        public PedidoController(IPedidoService pedidoService) 
        {
            this.PedidoService = pedidoService;
        }

        // GET api/pedido
        [HttpGet]
        public IActionResult Get()
        {
            var itens = this.PedidoService.ObterTodos();

            return Ok(itens);
        }

        // GET api/pedido/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if(id == 0){
                return BadRequest();
            }

            var pedido = this.PedidoService.MontarDetalhesPedido(id);
            return Ok(pedido);
        }

        // POST api/pedido
        [HttpPost]
        public IActionResult Post([FromBody] Pedido item)
        {
            if(item == null){
                return BadRequest();
            }

            item = this.PedidoService.MontarPizza(item);

            return Created($"api/pedido/{item.Id}", item);
        }

        // PUT api/pedido
        [HttpPut]
        public IActionResult Put([FromBody] Pedido item)
        {
            if(item == null){
                return BadRequest();
            }

            item = this.PedidoService.PersonalizarPizza(item);

            return Ok(item);
        }

        // DELETE api/pedido
        [HttpDelete]
        public IActionResult Delete()
        {
            this.PedidoService.RemoverTodos();

            return NoContent();
        }
    }
}
