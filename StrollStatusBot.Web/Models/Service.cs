using System;
using System.Threading;
using System.Threading.Tasks;
using GoogleSheetsManager;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace StrollStatusBot.Web.Models
{
    internal sealed class Service : IHostedService, IDisposable
    {
        public Service(IBot bot)
        {
            _bot = bot;

            string googleCredentialsJson = _bot.Config.GoogleCredentialsJson;
            if (string.IsNullOrWhiteSpace(googleCredentialsJson))
            {
                googleCredentialsJson = JsonConvert.SerializeObject(_bot.Config.GoogleCredentials);
            }
            _googleSheetsProvider = new Provider(googleCredentialsJson, ApplicationName, _bot.Config.GoogleSheetId);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var usersManager = new UsersManager(_bot.Client, _googleSheetsProvider, _bot.Config.GoogleRange);
            _bot.Initialize(usersManager);
            usersManager.LoadUsers();

            return _bot.Client.SetWebhookAsync(_bot.Config.Url, cancellationToken: cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _bot.Client.DeleteWebhookAsync(cancellationToken);
        }

        public void Dispose() => _googleSheetsProvider?.Dispose();

        private readonly IBot _bot;
        private readonly Provider _googleSheetsProvider;

        private const string ApplicationName = "DaresGameBot";
    }
}