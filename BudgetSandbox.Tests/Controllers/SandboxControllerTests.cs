using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BudgetSandbox.Api.Controllers;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Models.DTO;
using BudgetSandbox.Api.Services.Domain;
using System.Security.Claims;

namespace BudgetSandbox.Tests.Controllers
{
    public class SandboxControllerTests
    {
        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "randomuserId") }));
        Mock<ISandboxService> sandboxServiceMock = new Mock<ISandboxService>();
        SandboxController sandboxController;

        public SandboxControllerTests()
        {
            sandboxController = new SandboxController(sandboxServiceMock.Object);
            sandboxController.ControllerContext = new ControllerContext()
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
            var postResponse = await sandboxController.Post(new Sandbox { SandboxId = 1000 });
            var deleteResponse = await sandboxController.Delete(It.IsAny<int>());

            //Assert
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
            var postResponse = await sandboxController.Post(null);

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            sandboxServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Model_state_error_should_return_bad_request()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            var expectedResponse = new BadRequestResult();

            //Act
            sandboxController.ModelState.AddModelError("test", "This is a test error");
            var postResponse = await sandboxController.Post(new Sandbox());

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            sandboxServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await sandboxController.Get();

            //Assert
            sandboxServiceMock.Verify(m => m.GetAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
            sandboxServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Post_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await sandboxController.Post(new Sandbox());

            //Assert
            sandboxServiceMock.Verify(m => m.SaveAsync(It.IsAny<Sandbox>(), It.IsAny<string>()), Times.Once);
            sandboxServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Delete_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await sandboxController.Delete(It.IsAny<int>());

            //Assert
            sandboxServiceMock.Verify(m => m.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
            sandboxServiceMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Once);
            sandboxServiceMock.VerifyNoOtherCalls();
        }
    }
}
