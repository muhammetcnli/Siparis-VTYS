using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using alisveris.Models;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text;

namespace alisveris.Controllers
{
    [Authorize] // Sadece giriş yapmış kullanıcılar erişebilsin
    public class SiparisController : Controller
    {
        private readonly AppDbContext _context;

        public SiparisController(AppDbContext context)
        {
            _context = context;
        }

        // Siparişlerin listelendiği sayfa
        public async Task<IActionResult> Index()
        {
            var kullaniciEposta = User.Identity?.Name;
            var kullanici = await _context.Kullanici
                .FirstOrDefaultAsync(k => k.EPosta == kullaniciEposta);

            if (kullanici == null)
            {
                return RedirectToAction("Giris", "Account"); // Kullanıcı bulunamazsa giriş sayfasına yönlendirme
            }

            var siparisler = await _context.Siparis
                .Where(s => s.KullaniciID == kullanici.KullaniciID)
                .Include(s => s.Kargo)
                .Include(s => s.SiparisDetaylari)
                .ToListAsync();

            return View(siparisler);
        }

 [HttpGet]
    [Route("Admin/ExportOrdersToCsv")]
     public IActionResult ExportOrdersToCsv()
{
    var connectionString = "Server=DESKTOP-D4PHQS9\\SQLEXPRESS;Database=alisveris;Trusted_Connection=True;TrustServerCertificate=True;";
    var query = "SELECT * FROM Siparis";
    var dt = new DataTable();

    try
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("SQL bağlantısı başarılı.");
            var adapter = new SqlDataAdapter(query, connection);
            adapter.Fill(dt);
        }

        if (dt == null || dt.Rows.Count == 0)
        {
            return BadRequest("Tabloda hiçbir veri bulunamadı.");
        }

        var csvData = new StringBuilder();
        var columnNames = dt.Columns.Cast<DataColumn>()
                                    .Select(column => column.ColumnName)
                                    .ToArray();
        csvData.AppendLine(string.Join(",", columnNames));

        foreach (DataRow row in dt.Rows)
        {
            var fields = row.ItemArray
                .Select(field => field?.ToString()?.Replace(",", ";").Replace("\n", " ").Replace("\r", " ") ?? string.Empty)
                .ToArray();
            csvData.AppendLine(string.Join(",", fields));
        }

        var fileBytes = Encoding.UTF8.GetBytes(csvData.ToString());
        return File(fileBytes, "application/csv", "OrdersExport.csv");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Hata: " + ex.Message);
        return BadRequest("Bir hata oluştu: " + ex.Message);
    }
} 

        // Sipariş ekleme sayfası
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Sipariş ekleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Siparis model)
        {

            var kullaniciEposta = User.Identity?.Name;
            var kullanici = await _context.Kullanici
                .FirstOrDefaultAsync(k => k.EPosta == kullaniciEposta);

            // Kullanıcı doğrulama
            if (kullanici == null)
            {
                return RedirectToAction("Giris", "Account");
            }

            model.KullaniciID = kullanici.KullaniciID;
            model.SiparisTarihi = DateTime.Now; // Sipariş tarihi otomatik olarak şimdi olarak set ediliyor
            _context.Siparis.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Sipariş listeleme sayfasına yönlendir

        }

        // Sipariş düzenleme sayfası
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var siparis = await _context.Siparis.FindAsync(id);
            if (siparis == null)
            {
                return NotFound();
            }

            return View(siparis);
        }

         public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var siparis = await _context.Siparis.FindAsync(id);
            if (siparis == null)
            {
                return NotFound();
            }

            _context.Siparis.Remove(siparis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Sipariş güncelleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Siparis model)
        {
            if (id != model.SiparisID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Siparis.Any(e => e.SiparisID == model.SiparisID))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index)); // Sipariş listeleme sayfasına yönlendir
            }

            return View(model);
        }
    }
}
