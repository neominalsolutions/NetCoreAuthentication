using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// mvc uygulamalarında login bilgileri cookie ile saklanır
// defa
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
  opt.Cookie.HttpOnly = false; // secure sadece https kanalından aktif olcak.
  opt.Cookie.SameSite = SameSiteMode.Strict;
  //opt.Cookie.Domain = "www.a.com";
  opt.ExpireTimeSpan = TimeSpan.FromDays(30);
  opt.LoginPath = "/login";
  opt.LogoutPath = "/logout";
  opt.AccessDeniedPath = "/unauthorized";
  opt.SlidingExpiration = true; // 20 dk da bir oturumu arttır.
});

builder.Services.AddAuthorization(policy =>
{
  policy.AddPolicy("UserDeletePolicy", options =>
  {
    options.RequireAuthenticatedUser(); // kimlik doğrulaması gerekiyor
    options.RequireClaim("User", "Delete"); // User ClaimType Delete Value'suna sadece izin ver.
    options.RequireRole("Manager"); // Kullanıcı Manager Rolüne sahip olmalıdır.
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
app.UseAuthentication(); // kimlik doğrulaması middleware aktif hale getiririz.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
