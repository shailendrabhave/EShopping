namespace Common.Logging.Correlation
{
    public class CorrelationIdGenerator : ICorrelationIdGenerator
    {
        private string correlationId = Guid.NewGuid().ToString("D");   
        public string Get() => correlationId;

        public void Set(string correlationId)
        {
            this.correlationId = correlationId;
        }
    }
}
