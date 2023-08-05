using BudgetSandbox.Api.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSandbox.Tests.Models.DTO
{
    public class BucketDtoTests
    {
        public static IEnumerable<object[]> ValidModels => new List<object[]>
        {
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1"
                }
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 0m,
                    GoalAchieved = false
                }
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 100m,
                    GoalAchieved = false,
                    AccountBuckets = new List<AccountBucketDto>
                    {
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Amount = 100m
                        }
                    }
                }
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 100m,
                    GoalAchieved = false,
                    AccountBuckets = new List<AccountBucketDto>
                    {
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Percent = 1
                        }
                    }
                }
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 100m,
                    GoalAchieved = false,
                    AccountBuckets = new List<AccountBucketDto>
                    {
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Amount = 50m
                        },
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Amount = 50m
                        }
                    }
                }
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 100m,
                    GoalAchieved = false,
                    AccountBuckets = new List<AccountBucketDto>
                    {
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Percent = 0.5m
                        },
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Percent = 0.5m
                        }
                    }
                },
            }
        };

        public static IEnumerable<object[]> InvalidModels => new List<object[]>
        {
            new object[]
            {
                new BucketDto()
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 100m,
                    GoalAchieved = false
                }
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 100m,
                    GoalAchieved = false,
                    AccountBuckets = new List<AccountBucketDto>
                    {
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Amount = 200m
                        }
                    }
                }
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 100m,
                    GoalAchieved = false,
                    AccountBuckets = new List<AccountBucketDto>
                    {
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Percent = 0.5m
                        }
                    }
                }
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 100m,
                    GoalAchieved = false,
                    AccountBuckets = new List<AccountBucketDto>
                    {
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Amount = 150m
                        },
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Amount = 50m
                        }
                    }
                }
            },
            new object[]
            {
                new BucketDto
                {
                    Description = "Bucket 1",
                    Balance = 100m,
                    GoalAchieved = false,
                    AccountBuckets = new List<AccountBucketDto>
                    {
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Percent = 0.6m
                        },
                        new AccountBucketDto
                        {
                            AccountId = 1,
                            Percent = 0.5m
                        }
                    }
                },
            }
        };

        [Theory]
        [MemberData(nameof(ValidModels))]
        public void Should_be_valid(BucketDto model)
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
        public void Should_be_invalid(BucketDto model)
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
