namespace BudgetSandbox.Api.Models.DTO
{
    public class ReportCashFlowDto
    {
        public decimal YearlyCashFlow { get; set; }
        public decimal MonthlyCashFlow { get; set; }
        public decimal WeeklyCashFlow { get; set; }
        public decimal DailyCashFlow { get; set; }
        public decimal SavePercentage { get; set; }
    }
}
