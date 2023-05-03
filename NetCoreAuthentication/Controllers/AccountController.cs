using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NetCoreAuthentication.Models;
using System.Security.Claims;

namespace NetCoreAuthentication.Controllers
{
  public class AccountController : Controller
  {
    [HttpGet("login", Name ="loginRoute")]
    public IActionResult Login()
    {
      var model = new LoginInputModel();

      return View(model);
    }

    [HttpPost("login", Name ="loginRoute")]
    public async Task<IActionResult> Login(LoginInputModel model)
    {
      if(model.Email == "test@test.com" && model.Password == "Pass1234?")
      {

        // claim based authentication
        // kullanıcı hesabına ait özellikler
        // email,username,userId,admin,showReports,downloadFile
        var claim = new List<Claim>();
        claim.Add(new Claim("UserName", "test"));
        claim.Add(new Claim("Email", model.Email));
        claim.Add(new Claim("UserId", Guid.NewGuid().ToString()));
        claim.Add(new Claim("Role", "Admin"));

        //HttpContext.User.IsInRole("")

        // ClaimsIdentity bu sınfı üzerinden yukarıdaki cliamler ile login olabiliriz.
        // oturum açacak kullanıcı kimlik bilgisi
        var claimsIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme,nameType:"UserName",roleType:"Role");
        var principle = new ClaimsPrincipal(claimsIdentity);

        var authProps = new AuthenticationProperties
        {
          IsPersistent = model.RememberMe, // Oturum kalıcı olsun, false dersek oturum session bazlı tutulur. siteden ayrılınca tekrar login olmamız gerekir.
          AllowRefresh = true,

        };


        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle, authProps);

        return Redirect("/");


      }


      return View();
    }

    public async Task<IActionResult> LogOut()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Oturumdan güvenli çıkış yapmamızı sağlar.

      return Redirect("/login");
    }
  }
}
