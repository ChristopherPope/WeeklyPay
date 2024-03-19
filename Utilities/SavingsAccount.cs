using Microsoft.Extensions.Options;
using System.Text;
using WeeklyPay.Utilities.Interfaces;

namespace WeeklyPay.Utilities;
internal class SavingsAccount : ISavingsAccount
{
    private readonly WeeklyPayOptions options;
    private Dictionary<DateTime, decimal> record = new();

    public decimal Balance { get; private set; }

    public SavingsAccount(IOptions<WeeklyPayOptions> options)
    {
        this.options = options.Value;
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

    public Decimal Deduct(Decimal amount)
    {
        if (amount > Balance)
        {
            amount = Balance;
        }

        Balance -= amount;

        return amount;
    }

    public decimal Deposit(Decimal amount)
    {
        Balance += amount;

        return Balance;
    }

    public void RecordDay(DateTime date)
    {
        record.Add(date, Balance);
    }

    public void SetBalance(decimal balance)
    {
        Balance = balance;
    }
}
