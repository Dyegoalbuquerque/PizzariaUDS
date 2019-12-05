using Microsoft.AspNetCore.Mvc;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ItemRemovivelController: ControllerBase
    {
        private IItemRemovivelService ItemRemovivelService {get; set; }
        public ItemRemovivelController(IItemRemovivelService itemRemovivelService) 
        {
            this.ItemRemovivelService = itemRemovivelService;
        }

        // GET api/itemRemovivel
        [HttpGet]
        public IActionResult Get()
        {
            var itens = this.ItemRemovivelService.ObterTodos();

            return Ok(itens);
        }       
    }
}