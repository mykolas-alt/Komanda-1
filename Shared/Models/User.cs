using System.Security.Claims;

namespace Projektas.Shared.Models
{
    public class User
    {
        public string Name { get; set; }="";
        public string Surname { get; set; }="";
        public string Username { get; set; }="";
        public string Password { get; set; }="";

        public ClaimsPrincipal ToClaimsPrincipal()=>new(new ClaimsIdentity(new Claim[] {
            new (ClaimTypes.Name, Username),
            new (ClaimTypes.Hash, Password)
        },"UserAuth"));

        public static User FromClaimsPrincipal(ClaimsPrincipal principal) => new() {
            Username=principal.FindFirst(ClaimTypes.Name)?.Value ?? "",
            Password=principal.FindFirst(ClaimTypes.Hash)?.Value ?? ""
        };
    }
}
