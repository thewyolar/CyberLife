namespace CyberLife.Models;

public class User: BaseModel
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Patronymic { get; set; }
    
    public Role Role { get; set; }
    
    public string Email { get; set; }
    
    public List<Bot> bots { get; set; }
}