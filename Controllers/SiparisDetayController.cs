using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using alisveris.Models;
using System.Linq;
using System.Threading.Tasks;

namespace alisveris.Controllers
{
    [Authorize] // Sadece giriş yapmış kullanıcılar erişebilsin
    public class SiparisDetayController : Controller
    {
        private readonly AppDbContext _context;

        public SiparisDetayController(AppDbContext context)
        {
            _context = context;
        }

        // Belirli bir siparişin detaylarını listeleme
        public async Task<IActionResult> Index()
        {
            var kullaniciEposta = User.Identity?.Name;
            var kullanici = await _context.Kullanici
                .FirstOrDefaultAsync(k => k.EPosta == kullaniciEposta);

            if (kullanici == null)
            {
                return RedirectToAction("Giris", "Account"); // Kullanıcı bulunamazsa giriş sayfasına yönlendirme
            }

            // Sipariş detaylarını veritabanından al
            var siparisDetaylari = await _context.SiparisDetaylari
                .Include(s => s.Icerir)
                .Include(s => s.Siparis)
                .ToListAsync();

            return View(siparisDetaylari);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SiparisDetayi model)
        {

            var kullaniciEposta = User.Identity?.Name;
            var kullanici = await _context.Kullanici
                .FirstOrDefaultAsync(k => k.EPosta == kullaniciEposta);

            if (kullanici == null)
            {
                return RedirectToAction("Giris", "Account");
            }

            _context.SiparisDetaylari.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Sipariş detayları listeleme sayfasına yönlendir

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var siparisDetayi = await _context.SiparisDetaylari.FindAsync(id);
            if (siparisDetayi == null)
            {
                return NotFound();
            }

            _context.SiparisDetaylari.Remove(siparisDetayi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Sipariş düzenleme sayfası
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var siparisDetayi = await _context.SiparisDetaylari.FindAsync(id);
            if (siparisDetayi == null)
            {
                return NotFound();
            }

            return View(siparisDetayi);
        }

        // Sipariş güncelleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SiparisDetayi model)
        {
            if (id != model.DetayID)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.SiparisDetaylari.Any(e => e.DetayID == model.DetayID))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index)); // Sipariş listeleme sayfasına yönlendir
        }
    }
}
