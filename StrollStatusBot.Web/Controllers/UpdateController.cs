using Microsoft.AspNetCore.Mvc;
using StrollStatusBot.Web.Models;
using Telegram.Bot.Types;

namespace StrollStatusBot.Web.Controllers;

public sealed class UpdateController : Controller
{
    public OkResult Post([FromServices] BotSingleton singleton, [FromBody] Update update)
    {
        singleton.Bot.Update(update);
        return Ok();
    }
}