namespace EventBus.Messages.Events
{
    public class BaseIntegration
    {
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }
        public BaseIntegration()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public BaseIntegration(Guid id, DateTime creationTime)
        {
            Id = id;
            CreationDate = creationTime;
        }
    }
}
