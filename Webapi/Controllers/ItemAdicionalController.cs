using Microsoft.AspNetCore.Mvc;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ItemAdicionalController : ControllerBase
    {
        private IItemAdicionalService ItemAdicionalService {get; set; }
        public ItemAdicionalController(IItemAdicionalService itemAdicionalService) 
        {
            this.ItemAdicionalService = itemAdicionalService;
        }

        // GET api/itemAdicional
        [HttpGet]
        public IActionResult Get()
        {
            var itens = this.ItemAdicionalService.ObterTodos();

            return Ok(itens);
        }
    }
}