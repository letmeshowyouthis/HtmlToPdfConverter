namespace Infra.EasySaga
{
    internal interface ISagaBuilder<T>
    {
        public void AddTransaction(ISagaTransactionAsync<T> transaction);

        public ISagaScenario<T> Build();
    }
}
