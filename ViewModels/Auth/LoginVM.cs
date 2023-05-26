using System.ComponentModel.DataAnnotations;

public class LoginVM
{
    [Required, MaxLength(100)]
    public string UserName { get; set; }
    [Required, MinLength(8), DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RemmeberMe { get; set; }    
}
