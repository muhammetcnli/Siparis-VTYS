using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

public class AdminController : Controller
{
    private readonly string _backupFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "backups");

    public AdminController()
    {
        // Eğer "backups" klasörü yoksa oluştur
        if (!Directory.Exists(_backupFolderPath))
        {
            Directory.CreateDirectory(_backupFolderPath);
        }
    }

    // Yedekle
    [HttpPost]
    public IActionResult Yedekle()
    {
        try
        {
            string backupFileName = $"Backup_{DateTime.Now:yyyyMMddHHmmss}.bak";
            string backupFilePath = Path.Combine("C:\\DatabaseBackups", backupFileName);

            // Veritabanını yedekleme komutu (SQL Server için)
            string sqlBackupCommand = $"BACKUP DATABASE [alisveris] TO DISK = '{backupFilePath}'";

            // SQL komutunu çalıştır
            ExecuteSqlCommand(sqlBackupCommand);

            TempData["Message"] = "Veritabanı başarıyla yedeklendi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Yedekleme sırasında bir hata oluştu: {ex.Message}";
        }

        return RedirectToAction("Index", "Home");
    }

    // Yedekten Dön
    [HttpPost]
    public IActionResult YedektenDon(IFormFile selectedFilePath)
    {
        try
        {
            if (selectedFilePath == null || selectedFilePath.Length == 0)
            {
                throw new Exception("Bir dosya seçmelisiniz.");
            }


            string fileName = Path.GetFileName(selectedFilePath.FileName);
            string filePath = Path.Combine("C:\\DatabaseUpload", fileName);

            if (Path.GetExtension(fileName) != ".bak")
            {
                throw new Exception("Sadece .bak dosyaları kabul edilir.");
            }


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                selectedFilePath.CopyTo(stream);
            }

            string connectionString = "Server=DESKTOP-D4PHQS9\\SQLEXPRESS;Database=alisveris;Trusted_Connection=True;TrustServerCertificate=True;";

            string restoreQuery = $@"
            USE master;
            ALTER DATABASE [alisveris] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
            RESTORE DATABASE [alisveris]
            FROM DISK = '{filePath}'
            WITH REPLACE, RECOVERY;
            ALTER DATABASE [alisveris] SET MULTI_USER;
        ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(restoreQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            TempData["Message"] = "Veritabanı başarıyla geri yüklendi.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Yedekten dönme sırasında bir hata oluştu: {ex.Message}";
        }

        return RedirectToAction("Index", "Home");
    }

    // SQL komutu çalıştırma
    private void ExecuteSqlCommand(string sqlCommand)
    {
        using (var connection = new SqlConnection("Server=DESKTOP-D4PHQS9\\SQLEXPRESS;Database=alisveris;Trusted_Connection=True;TrustServerCertificate=True;"))
        {
            using (var command = new SqlCommand(sqlCommand, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
