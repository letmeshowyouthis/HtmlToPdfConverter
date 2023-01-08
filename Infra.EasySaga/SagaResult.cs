namespace Infra.EasySaga
{
    public class SagaResult
    {
        public bool IsSuccess { get; private set; }
        public Exception? TransactionException { get; private set; }
        public Exception? RollbackException { get; private set; }

        private SagaResult() { }

        public static SagaResult Success()
        {
            return new SagaResult
            {
                IsSuccess = true,
                TransactionException = null,
                RollbackException = null
            };
        }

        public static SagaResult TransactionAborted(Exception transactionException)
        {
            return new SagaResult
            {
                IsSuccess = false,
                TransactionException = transactionException,
                RollbackException = null
            };
        }

        public static SagaResult TransactionCrashed(Exception transactionException, Exception rollbackException)
        {
            return new SagaResult
            {
                IsSuccess = false,
                TransactionException = transactionException,
                RollbackException = rollbackException
            };
        }
    }
}
