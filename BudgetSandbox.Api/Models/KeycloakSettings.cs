namespace BudgetSandbox.Api.Models
{
    public class KeycloakSettings
    {
        public string BaseUrl { get; set; }
        public string Realm { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
