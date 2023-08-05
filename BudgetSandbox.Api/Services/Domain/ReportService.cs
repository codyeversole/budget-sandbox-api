using BudgetSandbox.Api.Models.Constants;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Models.DTO;
using BudgetSandbox.Api.Services.Data;

namespace BudgetSandbox.Api.Services.Domain
{
    public interface IReportService
    {
        Task<ReportCashFlowDto> ReportCashFlow(int sandboxId);
    }

    public class ReportService : IReportService
    {
        private readonly IRepositoryService<CashFlowItem> repositoryService;
        private const decimal daysInYear = 365;
        private const decimal weeksInYear = 52.1429m;
        private const decimal weeksInMonth = 4.34524m;
        private const decimal daysInMonth = 30.4167m;


        public ReportService(IRepositoryService<CashFlowItem> repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        public async Task<ReportCashFlowDto> ReportCashFlow(int sandboxId)
        {
            var cashFlowItems = await repositoryService.GetMultipleAsync(a => a.SandboxId == sandboxId);

            decimal dailyCashFlow = 0;
            dailyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Yearly, daysInYear);
            dailyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Monthly, daysInMonth);
            dailyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.SemiMonthly, (daysInMonth / 2));
            dailyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.BiWeekly, 14);
            dailyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Weekly, 7);
            dailyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Daily, 1);

            decimal weeklyCashFlow = 0;
            weeklyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Yearly, weeksInYear);
            weeklyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Monthly, weeksInMonth);
            weeklyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.SemiMonthly, (weeksInMonth / 2));
            weeklyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.BiWeekly, 2);
            weeklyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Weekly, 1);
            weeklyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Daily, 1) * 7;

            decimal monthlyCashFlow = 0;
            monthlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Yearly, 12);
            monthlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Monthly, 1);
            monthlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.SemiMonthly, 1) * 2;
            monthlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.BiWeekly, 1) * (weeksInMonth / 2);
            monthlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Weekly, 1) * weeksInMonth;
            monthlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Daily, 1) * daysInMonth;

            decimal yearlyCashFlow = 0;
            yearlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Yearly, 1);
            yearlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Monthly, 1) * 12;
            yearlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.SemiMonthly, 1) * 24;
            yearlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.BiWeekly, 1) * (weeksInYear / 2);
            yearlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Weekly, 1) * weeksInYear;
            yearlyCashFlow += GetAmountForFrequency(cashFlowItems, CashFlowFrequency.Daily, 1) * daysInYear;

            decimal dailyIncome = 0;
            var positiveCashFlow = cashFlowItems.Where(cfi => cfi.Positive).ToList();
            dailyIncome += GetAmountForFrequency(positiveCashFlow, CashFlowFrequency.Yearly, daysInYear);
            dailyIncome += GetAmountForFrequency(positiveCashFlow, CashFlowFrequency.Monthly, daysInMonth);
            dailyIncome += GetAmountForFrequency(positiveCashFlow, CashFlowFrequency.SemiMonthly, (daysInMonth / 2));
            dailyIncome += GetAmountForFrequency(positiveCashFlow, CashFlowFrequency.BiWeekly, 14);
            dailyIncome += GetAmountForFrequency(positiveCashFlow, CashFlowFrequency.Weekly, 7);
            dailyIncome += GetAmountForFrequency(positiveCashFlow, CashFlowFrequency.Daily, 1);

            decimal savePercentage = dailyIncome > 0 ? dailyCashFlow / dailyIncome : 0;

            return new ReportCashFlowDto
            {
                YearlyCashFlow = Math.Round(yearlyCashFlow, 2),
                MonthlyCashFlow = Math.Round(monthlyCashFlow, 2),
                WeeklyCashFlow = Math.Round(weeklyCashFlow, 2),
                DailyCashFlow = Math.Round(dailyCashFlow, 2),
                SavePercentage = Math.Round(savePercentage, 2)
            };
        }

        private decimal GetAmountForFrequency(List<CashFlowItem> items, string frequency, decimal divisor)
        {
            decimal total = 0;

            foreach (var item in items.Where(c => c.Frequency == frequency))
            {
                total += GetAmount(item, divisor);
            }

            return total;
        }

        private decimal GetAmount(CashFlowItem item, decimal divisor)
        {
            decimal amount = item.Amount / divisor;

            if (item.Positive)
            {
                return amount;
            }
            else
            {
                return amount * -1;
            }
        }
    }
}
