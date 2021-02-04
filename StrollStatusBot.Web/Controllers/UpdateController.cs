using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StrollStatusBot.Web.Models;
using Telegram.Bot.Types;

namespace StrollStatusBot.Web.Controllers
{
    public sealed class UpdateController : Controller
    {
        public async Task<OkResult> Post([FromBody]Update update, [FromServices]BotSingleton singleton)
        {
            await singleton.Bot.UpdateAsync(update);
            return Ok();
        }
    }
}
