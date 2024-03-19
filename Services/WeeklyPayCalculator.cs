using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WeeklyPay.Extensions;
using WeeklyPay.Utilities;
using WeeklyPay.Utilities.Interfaces;

namespace WeeklyPay.Services;
internal class WeeklyPayCalculator : IHostedService
{
    private readonly WeeklyPayOptions options;
    private readonly ICheckingAccount checking;
    private readonly ISavingsAccount savings;
    private int bomPayday;
    private int eomPayday;

    public WeeklyPayCalculator(IOptions<WeeklyPayOptions> options,
        ICheckingAccount checking,
        ISavingsAccount savings)
    {
        this.checking = checking;
        this.savings = savings;
        this.options = options.Value;


        this.options.BomPayday = 6;
        this.options.EomPayday = 21;
        this.options.NetPay = 5500;
        this.options.BeginningSavingsBalance = 15000;
        this.options.WeeklyPay = 3000;
        this.options.StartDate = new DateTime(2024, 11, 30);
        this.options.FedTaxReturnDay = new DateTime(2025, 3, 1);
        this.options.StateTaxReturnDay = new DateTime(2025, 3, 15);
        this.options.FedTaxReturn = 6000;
        this.options.StateTaxReturn = 2000;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        savings.SetBalance(options.BeginningSavingsBalance);
        checking.SetBalance(options.BeginningCheckingBalance);

        var date = options.StartDate;
        var days = 365;
        var doRecordSavingsDay = false;
        var doRecordCheckingDay = false;
        while (days > 0)
        {
            doRecordCheckingDay = false;
            doRecordSavingsDay = false;

            if (date.Day == 1)
            {
                bomPayday = CalculatePayday(date.AddDays(options.BomPayday - 1));
                eomPayday = CalculatePayday(date.AddDays(options.EomPayday - 1));
            }

            if (IsStateReturnDay(date))
            {
                savings.Deposit(options.StateTaxReturn);
                doRecordSavingsDay = true;
            }

            if (IsFedReturnDay(date))
            {
                savings.Deposit(options.FedTaxReturn);
                doRecordSavingsDay = true;
            }

            if (IsPayday(date))
            {
                savings.Deposit(options.NetPay);
                doRecordSavingsDay = true;
            }

            if (date.DayOfWeek == DayOfWeek.Friday)
            {
                var depositAmount = savings.Deduct(options.WeeklyPay);
                checking.Deposit(depositAmount, date);

                doRecordSavingsDay = true;
                doRecordCheckingDay = true;
            }

            if (doRecordCheckingDay)
            {
                checking.RecordDay(date);
                checking.SetBalance(0);
            }
            if (doRecordSavingsDay)
            {
                savings.RecordDay(date);
            }

            days--;
            date = date.AddDays(1);
        }

        var savingsRecord = savings.GetRecord();
        var checkingRecord = checking.GetRecord();

        File.WriteAllText(@"E:\Temp\savings-record.log", savingsRecord);
        File.WriteAllText(@"E:\Temp\checking-record.log", checkingRecord);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private bool IsFedReturnDay(DateTime date)
    {
        return date == options.FedTaxReturnDay;
    }

    private bool IsStateReturnDay(DateTime date)
    {
        return date == options.StateTaxReturnDay;
    }

    private bool IsPayday(DateTime date)
    {
        return (date.Day == bomPayday || date.Day == eomPayday);
    }

    private int CalculatePayday(DateTime fromDate)
    {
        var payday = fromDate;
        while (payday.IsBankingWeekend() || payday.IsFederalHoliday())
        {
            payday = payday.AddDays(-1);
        }

        return payday.Day;
    }
}
