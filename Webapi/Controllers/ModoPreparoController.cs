using Microsoft.AspNetCore.Mvc;
using Webapi.Domain.Services.Abstract;
using Webapi.Entities;

namespace Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ModoPreparoController : ControllerBase
    {
        private IModoPreparoService ModoPreparoService {get; set; }
        public ModoPreparoController(IModoPreparoService modoPreparoService) 
        {
            this.ModoPreparoService = modoPreparoService;
        }

        // GET api/modoPreparo
        [HttpGet]
        public IActionResult Get()
        {
            var itens = this.ModoPreparoService.ObterTodos();

            return Ok(itens);
        }
    }
}