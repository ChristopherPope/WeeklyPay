namespace WeeklyPay.Utilities;
public sealed class WeeklyPayOptions
{
    public DateTime StartDate { get; set; }
    public DateTime FedTaxReturnDay { get; set; }
    public DateTime StateTaxReturnDay { get; set; }
    public int BomPayday { get; set; }
    public int EomPayday { get; set; }
    public decimal NetPay { get; set; }
    public decimal WeeklyPay { get; set; }
    public decimal FedTaxReturn { get; set; }
    public decimal StateTaxReturn { get; set; }
    public decimal BeginningSavingsBalance { get; set; }
    public decimal BeginningCheckingBalance { get; set; }
}
