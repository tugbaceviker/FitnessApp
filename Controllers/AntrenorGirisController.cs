using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp1.Controllers
{
    public class AntrenorGirisController : Controller
    {
        private readonly Context _context;
        public AntrenorGirisController(Context context)
        {

            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public List<antrenor1> TumAntrenorleriGetir()
        {
            List<antrenor1> antrenorler = _context.Antrenorler.ToList();
            return antrenorler;
        }



        [HttpPost]
        public IActionResult GirisKontrol(string kullaniciEposta, int id)
        {
            List<antrenor1> tumAntrnorler = TumAntrenorleriGetir();

            foreach (var item in tumAntrnorler)
            {

                if (kullaniciEposta == item.e_posta && id == item.antrenor_id)
                {


                    return RedirectToAction("Index", "AntrenorAnaPanel", new { antrenorId = id });
                }
            }


            // Giriş başarısız ise, aynı sayfaya hata mesajıyla birlikte yönlendir.
            ViewBag.HataMesaji = "Giriş başarısız. Lütfen bilgilerinizi kontrol edin.";
            return View("Index");
        }
    }
}