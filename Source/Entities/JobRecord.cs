using MessagePack;

namespace Dom;

sealed class JobRecord : Entity, IJobStorageRecord
{
    public string QueueID { get; set; }
    public DateTime ExecuteAfter { get; set; }
    public DateTime ExpireOn { get; set; }
    public bool IsComplete { get; set; }
    public int FailureCount { get; set; }

    static JobRecord()
    {
        MessagePackSerializer.DefaultOptions = MessagePack.Resolvers.ContractlessStandardResolver.Options;
    }

    public byte[] CommandMsgPack { get; set; }

    TCommand IJobStorageRecord.GetCommand<TCommand>()
        => MessagePackSerializer.Deserialize<TCommand>(CommandMsgPack);

    void IJobStorageRecord.SetCommand<TCommand>(TCommand command)
        => CommandMsgPack = MessagePackSerializer.Serialize(command);

    [Ignore]
    public object Command { get; set; }
}