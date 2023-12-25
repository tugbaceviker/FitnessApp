using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FitnessApp1.Controllers
{
    public class AdminGirisController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GirisKontrol(string username, string password)
        {
            // Admin adı ve şifresini kontrol et
            if (username == "yonetici1" && password == "sifre1")
            {
                // Giriş başarılıysa belirlediğiniz view'e yönlendir
                return RedirectToAction("Index", "AdminPaneli"); // Örnek olarak "Home" controller'ının "AdminPaneli" action'ına yönlendiriyoruz.
            }

            // Giriş başarısızsa aynı sayfada hata mesajıyla geri dön
            ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
            return View("Index");
        }
    }
}

