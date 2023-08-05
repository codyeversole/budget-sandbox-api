using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BudgetSandbox.Api.Controllers;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Models.DTO;
using BudgetSandbox.Api.Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSandbox.Tests.Controllers
{
    public class BucketControllerTests
    {
        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.NameIdentifier, "randomuserId") }));

        Mock<ISandboxService> sandboxServiceMock = new Mock<ISandboxService>();
        Mock<IBucketService> bucketServiceMock = new Mock<IBucketService>();
        Mock<IAccountService> accountServiceMock = new Mock<IAccountService>();
        BucketController bucketController;

        public BucketControllerTests()
        {
            bucketController = new BucketController(bucketServiceMock.Object, sandboxServiceMock.Object, accountServiceMock.Object);
            bucketController.ControllerContext = new ControllerContext()
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
            var getAllResponse = await bucketController.GetAll(It.IsAny<int>());
            var getResponse = await bucketController.Get(It.IsAny<int>());
            var postResponse = await bucketController.Post(new BucketDto());
            var deleteResponse = await bucketController.Delete(It.IsAny<int>());

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
            var postResponse = await bucketController.Post(null);

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            bucketServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Model_state_error_should_return_bad_request()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            var expectedResponse = new BadRequestResult();

            //Act
            bucketController.ModelState.AddModelError("test", "This is a test error");
            var postResponse = await bucketController.Post(new BucketDto());

            //Assert
            postResponse.Should().BeEquivalentTo(expectedResponse);
            bucketServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAll_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await bucketController.GetAll(It.IsAny<int>());

            //Assert
            bucketServiceMock.Verify(m => m.GetAllAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            bucketServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Get_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await bucketController.Get(It.IsAny<int>());

            //Assert
            bucketServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            bucketServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Post_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var postResponse = await bucketController.Post(new BucketDto());

            //Assert
            bucketServiceMock.Verify(m => m.SaveAsync(It.IsAny<Bucket>()), Times.Once);
            bucketServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Delete_should_invoke_method()
        {
            //Arrange
            sandboxServiceMock.Setup(b => b.HasAccessAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);
            bucketServiceMock.Setup(a => a.GetAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(new Bucket());

            //Act
            var postResponse = await bucketController.Delete(It.IsAny<int>());

            //Assert
            bucketServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
            bucketServiceMock.Verify(m => m.DeleteAsync(It.IsAny<Bucket>()), Times.Once);
            bucketServiceMock.VerifyNoOtherCalls();
        }
    }
}
