using System;
using Microsoft.Extensions.Options;
using StrollStatusBot.Bot;

namespace StrollStatusBot.Web.Models
{
    public sealed class BotSingleton : IDisposable
    {
        internal readonly Bot.Bot Bot;

        public BotSingleton(IOptions<Config> options) => Bot = new Bot.Bot(options.Value);

        public void Dispose() => Bot.Dispose();
    }
}