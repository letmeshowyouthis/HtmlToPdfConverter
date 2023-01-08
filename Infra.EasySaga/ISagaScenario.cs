namespace Infra.EasySaga
{
    public interface ISagaScenario<T>
    {
        Task<SagaResult> ExecuteAllAsync(T context);
    }
}
