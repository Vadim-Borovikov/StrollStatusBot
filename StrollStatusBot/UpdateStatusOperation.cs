using System.Threading.Tasks;
using AbstractBot.Operations;
using StrollStatusBot.Users;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StrollStatusBot;

internal sealed class UpdateStatusOperation : Operation
{
    protected override byte MenuOrder => 3;

    public UpdateStatusOperation(Bot bot, Manager manager) : base(bot)
    {
        MenuDescription = "*любой другой текст* – записать в таблицу как текущий статус";
        _manager = manager;
    }

    protected override async Task<ExecutionResult> TryExecuteAsync(Message message, long senderId)
    {
        if ((message.Type != MessageType.Text) || string.IsNullOrWhiteSpace(message.Text))
        {
            return ExecutionResult.UnsuitableOperation;
        }

        if (!IsAccessSuffice(senderId))
        {
            return ExecutionResult.InsufficentAccess;
        }

        await _manager.AddStatus(message.Chat, message.Text);
        return ExecutionResult.Success;
    }

    private readonly Manager _manager;
}