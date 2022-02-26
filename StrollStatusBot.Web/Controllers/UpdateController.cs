﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StrollStatusBot.Web.Models;
using Telegram.Bot.Types;

namespace StrollStatusBot.Web.Controllers;

public sealed class UpdateController : Controller
{
    public async Task<OkResult> Post([FromServices] BotSingleton singleton, [FromBody] Update update)
    {
        await singleton.Bot.UpdateAsync(update);
        return Ok();
    }
}