using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace CyberLife.Models;

public class User: IdentityUser
{
    [AllowNull] 
    public string FirstName { get; set; } = "";
    [AllowNull]
    public string LastName { get; set; } = "";
    
    public List<PerceptronModel> bots { get; set; }
}