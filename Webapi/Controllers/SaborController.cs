using Microsoft.AspNetCore.Mvc;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class SaborController : ControllerBase
    {
        private ISaborService SaborService {get; set; }
        public SaborController(ISaborService saborService) 
        {
            this.SaborService = saborService;
        }

        // GET api/sabor
        [HttpGet]
        public IActionResult Get()
        {
            var itens = this.SaborService.ObterTodos();

            return Ok(itens);
        }
    }
}