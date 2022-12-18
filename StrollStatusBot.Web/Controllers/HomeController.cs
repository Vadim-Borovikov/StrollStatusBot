using StrollStatusBot.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace StrollStatusBot.Web.Controllers;

[Route("")]
public sealed class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index([FromServices] BotSingleton singleton) => View(singleton.Bot.User);
}