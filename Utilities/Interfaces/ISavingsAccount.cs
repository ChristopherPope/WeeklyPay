namespace WeeklyPay.Utilities.Interfaces;
internal interface ISavingsAccount
{
    decimal Balance { get; }
    void SetBalance(decimal balance);
    decimal Deduct(decimal amount);
    decimal Deposit(decimal amount);
    string GetRecord();
    void RecordDay(DateTime date);
}
