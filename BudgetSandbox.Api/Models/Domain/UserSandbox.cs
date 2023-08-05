namespace BudgetSandbox.Api.Models.Domain
{
    public class UserSandbox
    {
        public int UserSandboxId { get; set; }
        public string UserId { get; set; } = null!;

        public int SandboxId { get; set; }
        public Sandbox Sandbox { get; set; } = null!;
    }
}
