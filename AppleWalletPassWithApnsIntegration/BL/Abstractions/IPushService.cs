namespace BL.Abstractions;

public interface IPushService<in TMessage>
{
    Task PushMessage(TMessage message);
}