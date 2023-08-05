using BudgetSandbox.Api.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSandbox.Tests.Models.DTO
{
    public class CashFlowItemBucketDtoTests
    {
        public static IEnumerable<object[]> ValidModels => new List<object[]>
        {
            new object[] { new CashFlowItemBucketDto { BucketId = 1, Amount = 50m } },
            new object[] { new CashFlowItemBucketDto { BucketId = 1, Percent = 1 } }
        };

        public static IEnumerable<object[]> InValidModels => new List<object[]>
        {
            new object[] { new CashFlowItemBucketDto { BucketId = 1 } },
            new object[] { new CashFlowItemBucketDto { BucketId = 1, Amount = 50m, Percent = 1 } }
        };

        [Theory]
        [MemberData(nameof(ValidModels))]
        public void Should_be_valid(CashFlowItemBucketDto model)
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
        [MemberData(nameof(InValidModels))]
        public void Should_be_invalid(CashFlowItemBucketDto model)
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
