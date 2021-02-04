using System;
using Microsoft.Extensions.Options;

namespace StrollStatusBot.Web.Models
{
    public sealed class BotSingleton : IDisposable
    {
        internal readonly Bot Bot;

        public BotSingleton(IOptions<Config.Config> options) => Bot = new Bot(options.Value);

        public void Dispose() => Bot.Dispose();
    }
}