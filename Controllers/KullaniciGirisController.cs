using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Concrete;
using FitnessApp1;

namespace FitnessApp.Controllers
{


    public class KullaniciGirisController : Controller
    {

        private readonly Context _context;
        public KullaniciGirisController(Context context)
        {

            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public List<kullanici> TumKullanicilariGetir()
        {
            List<kullanici> kullanicilar = _context.Kullanicilar.ToList();
            return kullanicilar;
        }



        [HttpPost]
        public IActionResult GirisKontrol(string kullaniciEposta, string sifre)
        {
            List<kullanici> tumKullanicilar = TumKullanicilariGetir();

            foreach (var item in tumKullanicilar)
            {
                if (kullaniciEposta == item.e_posta && sifre == item.sifre)
                {
                    // Giriş başarılıysa, başka bir sayfaya yönlendir.
                    return RedirectToAction("Index", "KullaniciAnaPanel", new { id = item.kullanici_id, antrenor_id=item.antrenor_id});
                }
            }


            // Giriş başarısız ise, aynı sayfaya hata mesajıyla birlikte yönlendir.
            ViewBag.HataMesaji = "Giriş başarısız. Lütfen bilgilerinizi kontrol edin.";
            return View("Index");
        }


        [HttpPost]
        public IActionResult KayitKontrol(string kullaniciAd, string kullaniciSoyad, DateTime dogumTarih, bool cinsiyet, string kullaniciEposta, string telefon, string picture, string sifre, int hedef)
        {
            try
            {
                int kullaniciSayisi = _context.Kullanicilar.Count();
                kullaniciSayisi += 1;
                // Kullanıcı oluşturma işlemi
                kullanici yeniKullanici = new kullanici
                {
                    kullanici_ad = kullaniciAd,
                    kullanici_soyad = kullaniciSoyad,
                    dogum_tarih = dogumTarih,
                    cinsiyet = cinsiyet,
                    e_posta = kullaniciEposta,
                    telefon = telefon,
                    picture = picture ?? "",
                    sifre = sifre,
                    antrenor_id = 0, // Otomatik sıfır atanıyor
                    kullanici_id = kullaniciSayisi, // Otomatik olarak atanacak
                    hedef = hedef
                };


                _context.Kullanicilar.Add(yeniKullanici);
                _context.SaveChanges();

                // Başarılı kayıt durumunda, giriş sayfasına yönlendir
                return RedirectToAction("Index", "KullaniciAnaPanel", new { id = kullaniciSayisi });
            }
            catch (Exception ex)
            {
                // Hata durumunda, aynı sayfaya hata mesajıyla birlikte yönlendir
                ViewBag.KayitError = "Kayıt işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return View("Index");
            }
        }


    }

}
