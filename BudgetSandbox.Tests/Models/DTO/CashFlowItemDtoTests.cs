using BudgetSandbox.Api.Models.Constants;
using BudgetSandbox.Api.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSandbox.Tests.Models.DTO
{
    public class CashFlowItemDtoTests
    {
        public static IEnumerable<object[]> ValidModels => new List<object[]>
        {
            new object[] 
            { 
                new CashFlowItemDto 
                { 
                    Description = "Cash flow item",
                    Amount = 0m,
                    Frequency = CashFlowFrequency.Monthly                    
                } 
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 100m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemBuckets = new List<CashFlowItemBucketDto>
                    {
                        new CashFlowItemBucketDto
                        {
                            BucketId = 1,
                            Amount = 100m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 50m
                        },
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 50m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 50m
                        }
                    },
                    CashFlowItemBuckets = new List<CashFlowItemBucketDto>
                    {
                        new CashFlowItemBucketDto
                        {
                            BucketId = 1,
                            Amount = 50m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Percent = 1
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemBuckets = new List<CashFlowItemBucketDto>
                    {
                        new CashFlowItemBucketDto
                        {
                            BucketId = 1,
                            Percent = 1
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Percent = 0.5m
                        },
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Percent = 0.5m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Percent = 0.5m
                        }
                    },
                    CashFlowItemBuckets = new List<CashFlowItemBucketDto>
                    {
                        new CashFlowItemBucketDto
                        {
                            BucketId = 1,
                            Percent = 0.5m
                        }
                    }
                }
            }
        };

        public static IEnumerable<object[]> InvalidModels => new List<object[]>
        {
            new object[]
            {
                new CashFlowItemDto()
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 200m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemBuckets = new List<CashFlowItemBucketDto>
                    {
                        new CashFlowItemBucketDto
                        {
                            BucketId = 1,
                            Amount = 200m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 50m
                        },
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 150m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 50m
                        }
                    },
                    CashFlowItemBuckets = new List<CashFlowItemBucketDto>
                    {
                        new CashFlowItemBucketDto
                        {
                            BucketId = 1,
                            Amount = 150m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Percent = 1.1m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemBuckets = new List<CashFlowItemBucketDto>
                    {
                        new CashFlowItemBucketDto
                        {
                            BucketId = 1,
                            Percent = 1.1m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 0.6m
                        },
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 0.5m
                        }
                    }
                }
            },
            new object[]
            {
                new CashFlowItemDto
                {
                    Description = "Cash flow item",
                    Amount = 100m,
                    Frequency = CashFlowFrequency.Monthly,
                    CashFlowItemAccounts = new List<CashFlowItemAccountDto>
                    {
                        new CashFlowItemAccountDto
                        {
                            AccountId = 1,
                            Amount = 0.6m
                        }
                    },
                    CashFlowItemBuckets = new List<CashFlowItemBucketDto>
                    {
                        new CashFlowItemBucketDto
                        {
                            BucketId = 1,
                            Amount = 0.6m
                        }
                    }
                }
            }
        };

        [Theory]
        [MemberData(nameof(ValidModels))]
        public void Should_be_valid(CashFlowItemDto model)
        {
            //Arrange
            var validationResultList = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResultList);

            //Assert
            Assert.True(isValid);
            Assert.Empty(validationResultList);
        }

        [Theory]
        [MemberData(nameof(InvalidModels))]
        public void Should_be_invalid(CashFlowItemDto model)
        {
            //Arrange
            var validationResultList = new List<ValidationResult>();

            //Act
            bool isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResultList);

            //Assert
            Assert.False(isValid);
            Assert.NotEmpty(validationResultList);
        }
    }
}
