using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace CyberLife.Models;

public class User: IdentityUser
{

    public string FirstName { get; set; } = "";

    public string LastName { get; set; } = "";
    
    public IList<PerceptronModel> Bots { get; set; }
}