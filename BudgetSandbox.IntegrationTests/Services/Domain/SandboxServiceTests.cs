using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using BudgetSandbox.Api.Data;
using BudgetSandbox.IntegrationTests.Common;
using BudgetSandbox.Api.Models.Domain;
using BudgetSandbox.Api.Services.Data;
using BudgetSandbox.Api.Services.Domain;

namespace BudgetSandbox.IntegrationTests.Services.Domain
{
    [Collection("Database collection")]
    public class SandboxServiceTests : IDisposable
    {
        DatabaseFixture databaseFixture;
        BudgetSandboxContext context;
        SandboxService sandboxService;

        public SandboxServiceTests(DatabaseFixture databaseFixture)
        {
            this.databaseFixture = databaseFixture;
            context = this.databaseFixture.CreateContext();
            context.Database.BeginTransaction();

            RepositoryService<UserSandbox> userSandboxRepository = new(context);
            RepositoryService<Sandbox> sandboxRepository = new(context);
            sandboxService = new(sandboxRepository, userSandboxRepository);
        }

        public void Dispose()
        {
            context.ChangeTracker.Clear();
            context.Dispose();
        }

        [Fact]
        public async Task Should_have_access_to_sandbox()
        {
            //Arrange

            //Act
            bool hasAccess = await sandboxService.HasAccessAsync(databaseFixture.UserId, databaseFixture.Sandbox.SandboxId);

            //Assert
            Assert.True(hasAccess);
        }

        [Fact]
        public async Task Should_not_have_access_to_sandbox()
        {
            //Arrange            

            //Act
            bool hasAccess = await sandboxService.HasAccessAsync("baduserid", databaseFixture.Sandbox.SandboxId);

            //Assert
            Assert.False(hasAccess);
        }

        [Fact]
        public async Task Should_get_sandboxs_related_to_user()
        {
            //Arrange
            var expectedSandbox = await context.Sandboxs
                .Where(b => b.SandboxId == databaseFixture.Sandbox.SandboxId)
                .FirstOrDefaultAsync();

            //Act
            List<Sandbox> sandboxs = await sandboxService.GetAsync(databaseFixture.UserId);

            //Assert
            Assert.NotNull(sandboxs);
            Assert.Single(sandboxs);
            sandboxs[0].Should().BeEquivalentTo(expectedSandbox, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_return_empty_sandboxs()
        {
            //Arrange

            //Act
            List<Sandbox> sandboxs = await sandboxService.GetAsync("baduserid");

            //Assert
            Assert.NotNull(sandboxs);
            Assert.Empty(sandboxs);
        }

        [Fact]
        public async Task Should_create_sandbox()
        {
            //Arrange
            Sandbox sandbox = CreateSandbox();

            //Act
            await sandboxService.SaveAsync(sandbox, databaseFixture.UserId);

            //Assert
            var sandboxResult = await context.Sandboxs
                .Where(b => b.SandboxId == sandbox.SandboxId)
                .FirstOrDefaultAsync();

            Assert.NotNull(sandboxResult);
            sandboxResult.Should().BeEquivalentTo(sandbox, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_update_sandbox()
        {
            //Arrange
            Sandbox sandbox = CreateSandbox();
            context.Sandboxs.Add(sandbox);
            await context.SaveChangesAsync();

            sandbox.Description = "Updated Description";

            //Act
            await sandboxService.SaveAsync(sandbox, databaseFixture.UserId);

            //Assert
            var sandboxResult = await context.Sandboxs
              .Where(b => b.SandboxId == sandbox.SandboxId)
              .FirstOrDefaultAsync();

            Assert.NotNull(sandboxResult);
            sandboxResult.Should().BeEquivalentTo(sandbox, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public async Task Should_delete_sandbox()
        {
            //Arrange
            var sandbox = new Sandbox { Description = "New Sandbox" };
            context.Sandboxs.Add(sandbox);
            await context.SaveChangesAsync();

            //Act
            await sandboxService.DeleteAsync(sandbox.SandboxId);

            //Assert
            var sandboxResult = await context.Sandboxs
                .Where(b => b.SandboxId == sandbox.SandboxId)
                .FirstOrDefaultAsync();

            Assert.Null(sandboxResult);
        }

        private Sandbox CreateSandbox()
        {
            return new Sandbox
            {
                Description = "Family Sandbox"
            };
        }

    }
}
