using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// mvc uygulamalarýnda login bilgileri cookie ile saklanýr
// defa
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
  opt.Cookie.HttpOnly = false; // secure sadece https kanalýndan aktif olcak.
  opt.Cookie.SameSite = SameSiteMode.Strict;
  //opt.Cookie.Domain = "www.a.com";
  opt.ExpireTimeSpan = TimeSpan.FromDays(30);
  opt.LoginPath = "/login";
  opt.LogoutPath = "/logout";
  opt.AccessDeniedPath = "/unauthorized";
  opt.SlidingExpiration = true; // 20 dk da bir oturumu arttýr.
});

builder.Services.AddAuthorization(policy =>
{
  policy.AddPolicy("UserDeletePolicy", options =>
  {
    options.RequireAuthenticatedUser(); // kimlik doðrulamasý gerekiyor
    options.RequireClaim("User", "Delete"); // User ClaimType Delete Value'suna sadece izin ver.
    options.RequireRole("Manager"); // Kullanýcý Manager Rolüne sahip olmalýdýr.
  });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); // kimlik doðrulamasý middleware aktif hale getiririz.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
