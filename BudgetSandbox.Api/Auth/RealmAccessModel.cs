using System.Text.Json.Serialization;

namespace BudgetSandbox.Api.Auth
{
    public class RealmAccessModel
    {
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
    }
}
