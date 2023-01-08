namespace Infra.EasySaga
{
    public class SagaBuilder<T> : ISagaScenario<T>
    {
        private readonly List<ISagaTransactionAsync<T>> _transactions = new();

        public SagaBuilder<T> AddTransaction(ISagaTransactionAsync<T> transaction)
        {
            _transactions.Add(transaction);
            return this;
        }

        public async Task<SagaResult> ExecuteAllAsync(T context)
        {
            var completeTransactions = new Stack<ISagaTransactionAsync<T>>();
            foreach (var transaction in _transactions)
            {
                try
                {
                    await transaction.ExecuteAsync(context);
                    completeTransactions.Push(transaction);
                }
                catch (Exception transactionException)
                {
                    var rollbackException = await RollbackAllAsync(context, completeTransactions);
                    if (rollbackException == null)
                    {
                        return SagaResult.TransactionAborted(transactionException);
                    }
                    else
                    {
                        return SagaResult.TransactionCrashed(transactionException, rollbackException);
                    }

                }
            }
            return SagaResult.Success();
        }

        private static async Task<Exception?> RollbackAllAsync(T context, Stack<ISagaTransactionAsync<T>> completeTransactions)
        {
            while (completeTransactions.Count > 0)
            {
                var transaction = completeTransactions.Pop();
                try
                {
                    await transaction.RollbackAsync(context);
                }
                catch (Exception e)
                {
                    return e;
                }
            }
            return null;
        }
    }
}