namespace Infra.EasySaga
{
    public interface ISagaTransactionAsync<T>
    {
        Task ExecuteAsync(T context);
        Task RollbackAsync(T context);
    }
}
