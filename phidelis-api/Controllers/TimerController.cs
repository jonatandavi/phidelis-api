using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace phidelis_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimerController : ControllerBase
    {

        /// <summary>
        /// Altera o tempo de execução do Hosted Service.
        /// O tempo é em minutos e só é permitido números inteiros.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpGet("{time}")]
        public async Task<ActionResult<string>> ChangeTimer(int time)
        {
            GlobalData.Timer = time;
            return string.Format("Tempo alterado para {0}", time);
        }
    }
}
