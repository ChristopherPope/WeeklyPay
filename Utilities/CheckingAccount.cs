using System.Text;
using WeeklyPay.Utilities.Interfaces;

namespace WeeklyPay.Utilities;
internal class CheckingAccount : ICheckingAccount
{
    private Dictionary<DateTime, decimal> record = new();

    public decimal Balance { get; private set; }

    public void SetBalance(decimal balance)
    {
        Balance = balance;
    }

    public void Deposit(decimal amount, DateTime onDay)
    {
        Balance += amount;
    }

    public string GetRecord()
    {
        var report = new StringBuilder();
        foreach (var kvp in record)
        {
            report.Append($"{kvp.Key:ddd MM/dd/yyyy} ${kvp.Value:N}{Environment.NewLine}");
        }

        return report.ToString();
    }

    public void RecordDay(DateTime date)
    {
        record.Add(date, Balance);
    }
}
