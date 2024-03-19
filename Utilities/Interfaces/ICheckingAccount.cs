namespace WeeklyPay.Utilities.Interfaces;
internal interface ICheckingAccount
{
    decimal Balance { get; }
    void SetBalance(decimal balance);
    void Deposit(decimal amount, DateTime onDay);
    string GetRecord();
    void RecordDay(DateTime date);
}


