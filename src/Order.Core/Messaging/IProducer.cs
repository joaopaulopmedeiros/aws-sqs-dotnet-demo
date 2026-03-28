namespace Order.Core.Messaging;

public interface IProducer<TEvent>
{
    Task ProduceAsync(TEvent @event, CancellationToken cancellationToken);
}