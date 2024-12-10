using alisveris.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace alisveris.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> IndexAsync()
        {
            var kullaniciEposta = User.Identity?.Name;

            if (string.IsNullOrEmpty(kullaniciEposta))
            {
                // Kullanıcı oturumu açmamış, burada uygun bir yönlendirme yapılabilir
                return RedirectToAction("Giris", "Account"); // Örneğin, giriş sayfasına yönlendirme
            }
            // E-posta adresi ile kullanıcıyı buluyoruz
            var kullanici = await _context.Kullanici
                .FirstOrDefaultAsync(k => k.EPosta == kullaniciEposta);

            if (kullanici != null)
            {
                // Kullanıcı rolüne göre işlem yapıyoruz
                if (kullanici.Rol == "Admin")
                {
                    // Admin ise özel işlemler yapılabilir
                    ViewBag.IsAdmin = true;
                }
                else
                {
                    // Diğer kullanıcılar için işlemler
                    ViewBag.IsAdmin = false;
                }
            }
            else
            {
                // Eğer kullanıcı bulunamazsa hata mesajı gösterelim
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";
            }

            return View();
        }

        // Giriş Sayfası (GET)
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        // Giriş İşlemi (POST)
        [HttpPost]
        public async Task<IActionResult> GirisAsync(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {

                var user = _context.Kullanici.Where(x => x.EPosta == model.EPosta && x.Sifre == model.Sifre).FirstOrDefault();
                if (user != null)
                {
                    // başarılı, cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.EPosta),
                        new Claim("Ad", user.Ad),
                        new Claim(ClaimTypes.Role, user.Rol),
                    };

                    var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ClaimsIdentity),
                    new AuthenticationProperties { IsPersistent = true });

                    return RedirectToAction("GuvenliSayfa");
                }
                else
                {
                    ModelState.AddModelError("", "E-posta veya şifre yanlış.");
                }
            }
            return View(model);
        }

        [Authorize]
        public IActionResult GuvenliSayfa()
        {

            ViewBag.Name = HttpContext.User.Identity?.Name;
            return View();
        }

        // Kayıt Sayfası (GET)
        [HttpGet]
        public IActionResult Kayit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Giris", "Account");
        }

        // Kayıt İşlemi (POST)
        [HttpPost]
        public IActionResult Kayit(RegistrationViewModel model)
        {


            // Yeni kullanıcıyı oluştur
            var newUser = new Kullanici
            {
                EPosta = model.EPosta,
                Sifre = model.Sifre,
                Adres = model.Adres,
                Ad = model.Ad,
                Soyad = model.Soyad,
                Rol = "Kullanıcı"  // Otomatik olarak rolü "Kullanıcı" olarak belirliyoruz
            };

            // Kullanıcıyı veritabanına ekle
            _context.Kullanici.Add(newUser);


            _context.Database.ExecuteSqlRaw("DISABLE TRIGGER trg_Kullanici_Insert ON dbo.Kullanici;");
            _context.SaveChanges();
            _context.Database.ExecuteSqlRaw("ENABLE TRIGGER trg_Kullanici_Insert ON dbo.Kullanici;");


            // Kayıt başarılı, giriş sayfasına yönlendir
            TempData["SuccessMessage"] = "Kayıt başarılı. Lütfen giriş yapın.";
            return RedirectToAction("Giris");

        }

    }
}
