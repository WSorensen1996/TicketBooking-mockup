using Microsoft.AspNetCore.Mvc;
using PoCAPI.Models.DTO;
using PoCAPI.Models;
using PoCAPI.Services;
using PoCAPI.Data;

namespace PoCAPI.Controllers
{
    [Route("api/costumers")]
    [ApiController]
    public class CostumerController : ControllerBase
    {
        private readonly IQueueService _queueService;
        public CostumerController(IQueueService queueService)
        {
        
            _queueService = queueService;
        }

        [HttpPost]
        public async Task<ActionResult<CostumersDTO>> CreateCostumer([FromBody] CostumersDTO _costumersDTO)
        {


            await _queueService.SendMessageAsync(_costumersDTO, "costumerqueuetodb");
            await _queueService.SendMessageAsync(_costumersDTO, "costumerqueuetoCRM");
            return Accepted(_costumersDTO);
        }
    }
}
