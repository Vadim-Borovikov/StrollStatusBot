using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoogleSheetsManager;
using StrollStatusBot.Web.Models.Commands;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StrollStatusBot.Web.Models
{
    internal sealed class Bot : IDisposable
    {
        public Bot(Config.Config config)
        {
            _config = config;

            _client = new TelegramBotClient(_config.Token);

            string googleCredentialsJson = _config.GoogleCredentialsJson;
            if (string.IsNullOrWhiteSpace(googleCredentialsJson))
            {
                googleCredentialsJson = JsonConvert.SerializeObject(_config.GoogleCredentials);
            }
            _googleSheetsProvider = new Provider(googleCredentialsJson, ApplicationName, _config.GoogleSheetId);

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(_config.CultureInfoName);

            Utils.SetupTimeZoneInfo(_config.SystemTimeZoneId);
            Utils.SetupReplyMarkup();

            _usersManager = new UsersManager(_client, _googleSheetsProvider, _config.GoogleRange);
            _usersManager.LoadUsers();

            _commands = new List<Command>
            {
                new StartCommand(_config.InstructionLines)
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _client.SetWebhookAsync(_config.Url, cancellationToken: cancellationToken);
        }

        public Task UpdateAsync(Update update)
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

            Command command = _commands.FirstOrDefault(c => c.IsInvokingBy(message));

            return command != null
                ? command.ExecuteAsync(message.From.Id, _client)
                : _usersManager.AddStatus(message.From, message.Text);
        }

        public Task StopAsync(CancellationToken cancellationToken) => _client.DeleteWebhookAsync(cancellationToken);

        public void Dispose() => _googleSheetsProvider?.Dispose();

        public Task<Telegram.Bot.Types.User> GetUserAsunc() => _client.GetMeAsync();

        private readonly TelegramBotClient _client;
        private readonly Config.Config _config;
        private readonly List<Command> _commands;
        private readonly Provider _googleSheetsProvider;
        private readonly UsersManager _usersManager;

        private const string ApplicationName = "StrollStatusBot";
    }
}