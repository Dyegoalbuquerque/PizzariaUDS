using Microsoft.AspNetCore.Mvc;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ItemPrecoController: ControllerBase
    {
        private IItemPrecoService ItemPrecoService {get; set; }
        public ItemPrecoController(IItemPrecoService itemPrecoService) 
        {
            this.ItemPrecoService = itemPrecoService;
        }

        // GET api/itemPreco
        [HttpGet]
        public IActionResult Get()
        {
            var itens = this.ItemPrecoService.ObterTodos();

            return Ok(itens);
        }       
    }
}