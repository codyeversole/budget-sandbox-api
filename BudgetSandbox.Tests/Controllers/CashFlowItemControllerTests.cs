using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BudgetSandbox.Api.Controllers;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Models.DTO;
using BudgetSandbox.Api.Services.Domain;
using System.Collections;
using System.Security.Claims;

namespace BudgetSandbox.Tests.Controllers
{
    public class CashFlowItemControllerTests
    {
        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "randomuserId") }));
        Mock<ISandboxService> sandboxServiceMock = new Mock<ISandboxService>();
        Mock<ICashFlowItemService> cashFlowItemServiceMock = new Mock<ICashFlowItemService>();
        CashFlowItemController cashFlowItemController;

        public CashFlowItemControllerTests()
        {
            cashFlowItemController = new CashFlowItemController(cashFlowItemServiceMock.Object, sandboxServiceMock.Object);
            cashFlowItemController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Fact]
        public async Task Should_return_unauthorized()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

            var expectedResponse = new UnauthorizedResult();

            //Act
            var getAllResponse = await cashFlowItemController.GetAll(It.IsAny<int>());
            var getResponse = await cashFlowItemController.Get(It.IsAny<int>());
            var postResponse = await cashFlowItemController.Post(new CashFlowItemDto());
            var deleteResponse = await cashFlowItemController.Delete(It.IsAny<int>());

            //Assert
            getAllResponse.Result.Should().BeEquivalentTo(expectedResponse);
            getResponse.Result.Should().BeEquivalentTo(expectedResponse);
            postResponse.Should().BeEquivalentTo(expectedResponse);
            deleteResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Null_should_return_bad_request()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            var expectedResponse = new BadRequestResult();

            //Act
            var postResponse = await cashFlowItemController.Post(null);

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            cashFlowItemServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Model_state_error_should_return_bad_request()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            var expectedResponse = new BadRequestResult();

            //Act
            cashFlowItemController.ModelState.AddModelError("test", "This is a test error");
            var postResponse = await cashFlowItemController.Post(new CashFlowItemDto());

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            cashFlowItemServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAll_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await cashFlowItemController.GetAll(It.IsAny<int>());

            //Assert
            cashFlowItemServiceMock.Verify(m => m.GetAllAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            cashFlowItemServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await cashFlowItemController.Get(It.IsAny<int>());

            //Assert
            cashFlowItemServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            cashFlowItemServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Post_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await cashFlowItemController.Post(new CashFlowItemDto());

            //Assert
            cashFlowItemServiceMock.Verify(m => m.SaveAsync(It.IsAny<CashFlowItem>()), Times.Once);
            cashFlowItemServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Delete_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);
            cashFlowItemServiceMock.Setup(a => a.GetAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(new CashFlowItem());

            //Act
            var postResponse = await cashFlowItemController.Delete(It.IsAny<int>());

            //Assert
            cashFlowItemServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            cashFlowItemServiceMock.Verify(m => m.DeleteAsync(It.IsAny<CashFlowItem>()), Times.Once);
            cashFlowItemServiceMock.VerifyNoOtherCalls();
        }

    }
}
