using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using alisveris.Models;
using Microsoft.EntityFrameworkCore;

namespace alisveris.Controllers
{
    [Authorize]  // Sadece admin rolüyle giriş yapmış kullanıcılar erişebilir
    public class KullaniciController : Controller
    {
        private readonly AppDbContext _context;

        public KullaniciController(AppDbContext context)
        {
            _context = context;
        }




// Ad'a göre arama
    public IActionResult IndexByAd(string ad)
    {
        var kullanicilar = _context.Kullanici
            .Where(k => k.Ad.Contains(ad))
            .ToList();
        return View(kullanicilar);
    }

    // Soyad'a göre arama
    public IActionResult IndexBySoyad(string soyad)
    {
        var kullanicilar = _context.Kullanici
            .Where(k => k.Soyad.Contains(soyad))
            .ToList();
        return View(kullanicilar);
    }

    // Ad ve Soyad'a göre arama
    public IActionResult IndexByAdSoyad(string ad, string soyad)
    {
        var kullanicilar = _context.Kullanici
            .Where(k => k.Ad.Contains(ad) && k.Soyad.Contains(soyad))
            .ToList();
        return View(kullanicilar);
    }

    // Rol'e göre arama
    public IActionResult IndexByRol(string rol)
    {
        var kullanicilar = _context.Kullanici
            .Where(k => k.Rol.Contains(rol))
            .ToList();
        return View(kullanicilar);
    }

    // Tüm parametreler ile arama
    public IActionResult IndexByMultipleParams(string ad, string soyad, string rol)
    {
        var kullanicilar = _context.Kullanici
            .Where(k => (string.IsNullOrEmpty(ad) || k.Ad.Contains(ad)) &&
                        (string.IsNullOrEmpty(soyad) || k.Soyad.Contains(soyad)) &&
                        (string.IsNullOrEmpty(rol) || k.Rol.Contains(rol)))
            .ToList();
        return View(kullanicilar);
    }

        // Kullanıcıları görüntüle
        public async Task<IActionResult> Index(string search, string SortOrder)
    {
        var viewModel = new KullaniciViewModel
        {
            SearchQuery = search,
            SortOrder = SortOrder,
            Kullanicilar = await GetKullanicilar(search, SortOrder)
        };

        return View(viewModel);
    }

    private async Task<List<Kullanici>> GetKullanicilar(string search, string sortOrder)
    {
        var kullanicilar = _context.Kullanici.AsQueryable();

        // Arama işlemi
        if (!string.IsNullOrEmpty(search))
        {
            kullanicilar = kullanicilar.Where(k => k.Ad.Contains(search) || k.Soyad.Contains(search) || k.EPosta.Contains(search));
        }

        // Sıralama işlemi
        switch (sortOrder)
        {
            case "ad_desc":
                kullanicilar = kullanicilar.OrderByDescending(k => k.Ad);
                break;
            case "soyad_asc":
                kullanicilar = kullanicilar.OrderBy(k => k.Soyad);
                break;
            case "soyad_desc":
                kullanicilar = kullanicilar.OrderByDescending(k => k.Soyad);
                break;
            case "rol_asc":
                kullanicilar = kullanicilar.OrderBy(k => k.Rol);
                break;
            case "rol_desc":
                kullanicilar = kullanicilar.OrderByDescending(k => k.Rol);
                break;
            default:
                kullanicilar = kullanicilar.OrderBy(k => k.Ad); // Varsayılan sıralama: Ad'a göre
                break;
        }

        return await kullanicilar.ToListAsync();
    }

        // Yeni kullanıcı eklemek için sayfa
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Kullanici kullanici)
        {
            
                _context.Kullanici.Add(kullanici);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }



        // Kullanıcıyı sil
        public IActionResult Delete(int id)
        {
            var kullanici = _context.Kullanici.Find(id);
            if (kullanici != null)
            {
                _context.Kullanici.Remove(kullanici);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // Kullanıcıyı düzenlemek için sayfa
        public IActionResult Edit(int id)
        {
            var kullanici = _context.Kullanici.Find(id);
            if (kullanici == null)
            {
                return NotFound();
            }
            return View(kullanici);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Kullanici kullanici)
        {
            if (id != kullanici.KullaniciID)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(kullanici);
                    _context.SaveChanges();
                }
                catch
                {
                    ModelState.AddModelError("", "Bir hata oluştu.");
                }
                return RedirectToAction(nameof(Index));
            }
    }
}
