namespace NetCoreAuthentication.Models
{
  public class LoginInputModel
  {
    public string Email { get; set; } = "test@test.com";
    public string Password { get; set; } = "Pass1234?";

    public bool RememberMe { get; set; } = true;


  }
}
