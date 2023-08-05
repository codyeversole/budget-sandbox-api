using FluentAssertions;
using Moq;
using BudgetSandbox.Api.Models.Constants;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Models.DTO;
using BudgetSandbox.Api.Services.Data;
using BudgetSandbox.Api.Services.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSandbox.Tests.Services.Domain
{
    public class ReportServiceTests
    {
        public static IEnumerable<object[]> ValidCashFlows => new List<object[]>
        {
            new object[] { 
                new List<CashFlowItem> 
                { 
                    new CashFlowItem { Amount = 100, Frequency = CashFlowFrequency.Daily, Positive = true } 
                }, 
                new ReportCashFlowDto 
                { 
                    DailyCashFlow = 100,
                    WeeklyCashFlow = 700,
                    MonthlyCashFlow = 3_041.67m,
                    YearlyCashFlow = 36_500,
                    SavePercentage = 1
                }
            },
            new object[] {
                new List<CashFlowItem>
                {
                    new CashFlowItem { Amount = 700, Frequency = CashFlowFrequency.Weekly, Positive = true }
                },
                new ReportCashFlowDto
                {
                    DailyCashFlow = 100,
                    WeeklyCashFlow = 700,
                    MonthlyCashFlow = 3_041.67m,
                    YearlyCashFlow = 36_500.03m,
                    SavePercentage = 1
                }
            },
            new object[] {
                new List<CashFlowItem>
                {
                    new CashFlowItem { Amount = 1400, Frequency = CashFlowFrequency.BiWeekly, Positive = true }
                },
                new ReportCashFlowDto
                {
                    DailyCashFlow = 100,
                    WeeklyCashFlow = 700,
                    MonthlyCashFlow = 3_041.67m,
                    YearlyCashFlow = 36_500.03m,
                    SavePercentage = 1
                }
            },
            new object[] {
                new List<CashFlowItem>
                {
                    new CashFlowItem { Amount = 1400, Frequency = CashFlowFrequency.SemiMonthly, Positive = true }
                },
                new ReportCashFlowDto
                {
                    DailyCashFlow = 92.05m,
                    WeeklyCashFlow = 644.38m,
                    MonthlyCashFlow = 2_800,
                    YearlyCashFlow = 33_600,
                    SavePercentage = 1
                }
            },
            new object[] {
                new List<CashFlowItem>
                {
                    new CashFlowItem { Amount = 2800, Frequency = CashFlowFrequency.Monthly, Positive = true }
                },
                new ReportCashFlowDto
                {
                    DailyCashFlow = 92.05m,
                    WeeklyCashFlow = 644.38m,
                    MonthlyCashFlow = 2_800,
                    YearlyCashFlow = 33_600,
                    SavePercentage = 1
                }
            },
            new object[] {
                new List<CashFlowItem>
                {
                    new CashFlowItem { Amount = 36_500, Frequency = CashFlowFrequency.Yearly, Positive = true }
                },
                new ReportCashFlowDto
                {
                    DailyCashFlow = 100,
                    WeeklyCashFlow = 700,
                    MonthlyCashFlow = 3_041.67m,
                    YearlyCashFlow = 36_500,
                    SavePercentage = 1
                }
            },
            new object[] {
                new List<CashFlowItem>
                {
                    new CashFlowItem { Amount = 10, Frequency = CashFlowFrequency.Daily, Positive = true },
                    new CashFlowItem { Amount = 9, Frequency = CashFlowFrequency.Daily, Positive = false }
                },
                new ReportCashFlowDto
                {
                    DailyCashFlow = 1,
                    WeeklyCashFlow = 7,
                    MonthlyCashFlow = 30.42m,
                    YearlyCashFlow = 365,
                    SavePercentage = 0.1m
                }
            },
            new object[] {
                new List<CashFlowItem>
                {
                    new CashFlowItem { Amount = 1, Frequency = CashFlowFrequency.Daily, Positive = false }
                },
                new ReportCashFlowDto
                {
                    DailyCashFlow = -1,
                    WeeklyCashFlow = -7,
                    MonthlyCashFlow = -30.42m,
                    YearlyCashFlow = -365,
                    SavePercentage = 0
                }
            },
        };

        [Theory]
        [MemberData(nameof(ValidCashFlows))]
        public async void Should_be_equal(List<CashFlowItem> items, ReportCashFlowDto expected)
        {
            //Arrange
            Mock<IRepositoryService<CashFlowItem>> cashFlowItemRepositoryMock = new Mock<IRepositoryService<CashFlowItem>>();
            cashFlowItemRepositoryMock.Setup(c => c.GetMultipleAsync(It.IsAny<Expression<Func<CashFlowItem, bool>>>(), It.IsAny<bool>())).ReturnsAsync(items);

            var reportService = new ReportService(cashFlowItemRepositoryMock.Object);

            //Act
            var result = await reportService.ReportCashFlow(It.IsAny<int>());

            //Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
