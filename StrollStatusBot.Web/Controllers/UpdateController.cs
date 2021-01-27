using System.Linq;
using System.Threading.Tasks;
using StrollStatusBot.Web.Models;
using StrollStatusBot.Web.Models.Commands;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StrollStatusBot.Web.Controllers
{
    public sealed class UpdateController : Controller
    {
        public UpdateController(IBot bot) => _bot = bot;

        public async Task<OkResult> Post([FromBody]Update update)
        {
            await ProcessAsync(update);
            return Ok();
        }

        private Task ProcessAsync(Update update)
        {
            if (update?.Type != UpdateType.Message)
            {
                return Task.CompletedTask;
            }

            Message message = update.Message;

            if (message.Type != MessageType.Text)
            {
                return Task.CompletedTask;
            }

            Command command = _bot.Commands.FirstOrDefault(c => c.IsInvokingBy(message));

            return command != null
                ? command.ExecuteAsync(message.From.Id, _bot.Client)
                : _bot.UsersManager.AddStatus(message.From, message.Text);
        }

        private readonly IBot _bot;
    }
}
