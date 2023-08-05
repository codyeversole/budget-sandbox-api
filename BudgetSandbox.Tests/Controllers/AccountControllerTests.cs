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
    public class AccountControllerTests
    {
        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "randomuserId") }));
        Mock<ISandboxService> sandboxServiceMock = new Mock<ISandboxService>();
        Mock<IAccountService> accountServiceMock = new Mock<IAccountService>();
        AccountController accountController;

        public AccountControllerTests()
        {
            accountController = new AccountController(accountServiceMock.Object, sandboxServiceMock.Object);
            accountController.ControllerContext = new ControllerContext()
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
            var getAllResponse = await accountController.GetAll(It.IsAny<int>());
            var getResponse = await accountController.Get(It.IsAny<int>());
            var postResponse = await accountController.Post(new AccountDto());
            var deleteResponse = await accountController.Delete(It.IsAny<int>());

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
            var postResponse = await accountController.Post(null);

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            accountServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Model_state_error_should_return_bad_request()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            var expectedResponse = new BadRequestResult();

            //Act
            accountController.ModelState.AddModelError("test", "This is a test error");
            var postResponse = await accountController.Post(new AccountDto());

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            accountServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAll_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await accountController.GetAll(It.IsAny<int>());

            //Assert
            accountServiceMock.Verify(m => m.GetAllAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            accountServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await accountController.Get(It.IsAny<int>());

            //Assert
            accountServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            accountServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Post_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await accountController.Post(new AccountDto());

            //Assert
            accountServiceMock.Verify(m => m.SaveAsync(It.IsAny<Account>()), Times.Once);
            accountServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Delete_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);
            accountServiceMock.Setup(a => a.GetAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(new Account());

            //Act
            var postResponse = await accountController.Delete(It.IsAny<int>());

            //Assert
            accountServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            accountServiceMock.Verify(m => m.DeleteAsync(It.IsAny<Account>()), Times.Once);
            accountServiceMock.VerifyNoOtherCalls();
        }

    }
}
