using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// using alisveris.Data; // AppDbContext'inizin olduğu namespace
using alisveris.Models;
using System.Reflection.Metadata.Ecma335; // Kargo modelinin olduğu namespace

namespace alisveris.Controllers
{
    // Bu controller'daki tüm işlemler giriş yapılmış kullanıcılarla sınırlıdır
    [Authorize]
    public class KargoController : Controller
    {
        private readonly AppDbContext _context;

        public KargoController(AppDbContext context)
        {
            _context = context;
        }

        // Kargo tablosundaki tüm kayıtları listeleme

        public async Task<IActionResult> IndexAsync(string search, string Durum, string kargoSirketi)
        {
            var kargos = _context.Kargo.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                kargos = kargos.Where(k => k.TakipNo.Contains(search) || k.KargoSirketi.Contains(search));
            }

            // Durum filtresi
            if (!string.IsNullOrEmpty(Durum))
            {
                kargos = kargos.Where(k => k.Durum == Durum);
            }

            if (!string.IsNullOrEmpty(kargoSirketi))
            {
                kargos = kargos.Where(k => k.KargoSirketi == kargoSirketi);
            }

            kargos = kargos.OrderBy(k => k.KargoSirketi);

            // Filtrelenmiş kargoları liste olarak getiriyoruz
            ViewData["StatusQuery"] = Durum;
            var filteredKargos = await _context.Kargo.ToListAsync();

            return View(filteredKargos); // Filtrelenmiş veriyi View'e gönderiyoruz
        }

        // Yeni kargo ekleme sayfasını görüntüleme
        public IActionResult Create()
        {
            return View();
        }

        // Yeni kargo ekleme işlemi
        [HttpPost]
        public IActionResult Create(Kargo kargo)
        {

            if (_context.Kargo.Any(k => k.TakipNo == kargo.TakipNo))
            {
                ModelState.AddModelError("TakipNo", "Bu takip numarası zaten kullanılmakta.");
                return View(kargo);
            }

            var ilgiliSiparis = _context.Siparis.FirstOrDefault(s => s.SiparisID == kargo.SiparisID);

            if (ilgiliSiparis == null)
            {
                ModelState.AddModelError("SiparisID", "Geçerli bir Sipariş ID'si giriniz.");
                return View(kargo);
            }
            try
            {
                var yeniKargo = new Kargo
                {
                    KargoSirketi = kargo.KargoSirketi,
                    Durum = kargo.Durum,
                    TakipNo = kargo.TakipNo,
                    SiparisID = kargo.SiparisID,
                    Siparis = ilgiliSiparis
                };

                _context.Kargo.Add(yeniKargo);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Veri kaydedilirken bir hata oluştu: " + ex.Message);
                return View(kargo);
            }
        }



        // Kargo düzenleme sayfasını görüntüleme
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var kargo = await _context.Kargo.FindAsync(id);
            if (kargo == null)
            {
                return NotFound();
            }
            return View(kargo);
        }

        // Kargo düzenleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kargo kargo)
        {
            try
            {
                if (id != kargo.KargoID)
                {
                    return NotFound();
                }

                var mevcutKargo = await _context.Kargo.FirstOrDefaultAsync(k => k.KargoID == id);
                if (mevcutKargo == null)
                {
                    return NotFound();
                }

                _context.Entry(mevcutKargo).State = EntityState.Detached;
                // Alanları güncelle
                mevcutKargo.KargoSirketi = kargo.KargoSirketi;
                mevcutKargo.Durum = kargo.Durum;
                mevcutKargo.TakipNo = kargo.TakipNo;
                mevcutKargo.SiparisID = kargo.SiparisID;


                _context.Update(kargo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KargoExists(kargo.KargoID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }




        private bool KargoExists(int id)
        {
            return _context.Kargo.Any(k => k.KargoID == id);
        }
        // Kargo silme işlemi
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var kargo = await _context.Kargo.FindAsync(id);
            if (kargo == null)
            {
                return NotFound();
            }

            _context.Kargo.Remove(kargo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
