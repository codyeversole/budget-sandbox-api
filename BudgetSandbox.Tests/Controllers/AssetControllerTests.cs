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
    public class AssetControllerTests
    {
        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "randomuserId") }));
        Mock<ISandboxService> sandboxServiceMock = new Mock<ISandboxService>();
        Mock<IAssetService> assetServiceMock = new Mock<IAssetService>();
        AssetController assetController;

        public AssetControllerTests()
        {
            assetController = new AssetController(assetServiceMock.Object, sandboxServiceMock.Object);
            assetController.ControllerContext = new ControllerContext()
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
            var getAllResponse = await assetController.GetAll(It.IsAny<int>());
            var getResponse = await assetController.Get(It.IsAny<int>());
            var postResponse = await assetController.Post(new AssetDto());
            var deleteResponse = await assetController.Delete(It.IsAny<int>());

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
            var postResponse = await assetController.Post(null);

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            assetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Model_state_error_should_return_bad_request()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            var expectedResponse = new BadRequestResult();

            //Act
            assetController.ModelState.AddModelError("test", "This is a test error");
            var postResponse = await assetController.Post(new AssetDto());

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            assetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAll_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await assetController.GetAll(It.IsAny<int>());

            //Assert
            assetServiceMock.Verify(m => m.GetAllAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            assetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await assetController.Get(It.IsAny<int>());

            //Assert
            assetServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            assetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Post_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await assetController.Post(new AssetDto());

            //Assert
            assetServiceMock.Verify(m => m.SaveAsync(It.IsAny<Asset>()), Times.Once);
            assetServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Delete_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);
            assetServiceMock.Setup(a => a.GetAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(new Asset());

            //Act
            var postResponse = await assetController.Delete(It.IsAny<int>());

            //Assert
            assetServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            assetServiceMock.Verify(m => m.DeleteAsync(It.IsAny<Asset>()), Times.Once);
            assetServiceMock.VerifyNoOtherCalls();
        }

    }
}
