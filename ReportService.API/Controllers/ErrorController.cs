using Microsoft.AspNetCore.Mvc;

namespace ReportService.API.Controllers
{
    [ApiController]
    [Route("[controller]")] // API için /Error olarak ayarlandı, bu yüzden api/Error değil, /Error
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [Route("/Error")] // Global hata yakalayıcının yönlendirdiği yol
        protected IActionResult HandleError(Exception ex)
        {
            // Hata loglama işlemi burada yapılabilir.
            // Örneğin, bir loglama servisine (Serilog, NLog vb.) hata detaylarını gönderebilirsiniz.
            Console.WriteLine($"Hata loglandı: {ex.Message}");

            // Hata tipine göre özel durum kodları döndürme
            if (ex is ArgumentException)
            {
                // Geçersiz parametreler için 400 Bad Request
                return BadRequest(new { error = ex.Message });
            }
            if (ex is UnauthorizedAccessException)
            {
                // Yetkilendirme hatası için 401 Unauthorized
                return Unauthorized(new { error = "Bu işlemi gerçekleştirmek için yetkiniz yok." });
            }
            if (ex is KeyNotFoundException)
            {
                // Kaynak bulunamadığında 404 Not Found
                return NotFound(new { error = "Aradığınız kaynak bulunamadı." });
            }
            if (ex is InvalidOperationException)
            {
                // İşlem geçerli değilse 409 Conflict
                return Conflict(new { error = ex.Message });
            }

            // Diğer tüm beklenmeyen hatalar için 500 Internal Server Error
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                // Geliştirme ortamında daha detaylı hata döndürme
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }

            // Üretim ortamında daha genel bir hata mesajı döndürme
            return StatusCode(500, "Sunucuda beklenmeyen bir hata oluştu.");
        }


    }
}
