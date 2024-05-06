namespace EventBus.Messages.Events
{
    public class BaseIntegration
    {
        public string CorrelationId { get; set; }
        public DateTime CreationDate { get; private set; }
        public BaseIntegration()
        {
            CorrelationId = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
        }

        public BaseIntegration(Guid correlationId, DateTime creationTime)
        {
            CorrelationId = correlationId.ToString();
            CreationDate = creationTime;
        }
    }
}
