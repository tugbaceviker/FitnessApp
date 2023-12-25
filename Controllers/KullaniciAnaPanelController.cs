using DataAccessLayer.Concrete;
using FitnessApp1.Models;
using FitnessApp1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FitnessApp1.Controllers
{
    public class KullaniciAnaPanelController : Controller
    {
        private readonly Context _context;

        public KullaniciAnaPanelController(Context context)
        {
            _context = context;
        }

        public IActionResult Index(int id, int antrenor_id)
        {
            List<beslenme_programi> beslenmeProgramlari = _context.BeslenmeProgramlari.Where(bp => bp.kullanici_id == id).ToList();
            List<egzersiz_programi> egzersizProgramlari = _context.EgzersizProgramlari.Where(ep => ep.kullanici_id == id).ToList();
            List<kullanici> kullanici = _context.Kullanicilar.Where(ep => ep.kullanici_id == id).ToList();
            string picture = " ";
            foreach (var item in kullanici)
            {
                picture = item.picture;
            }
            ViewBag.picture = picture;

            ViewBag.antrenor_id = antrenor_id;
            ViewBag.kullanici_id = id;
            var model = new KullaniciProgramlariViewModel
            {
                BeslenmeProgramlari = beslenmeProgramlari,
                EgzersizProgramlari = egzersizProgramlari
            };

            return View(model);


        }
        public IActionResult Index3(int antrenor_id, int kullanici_id)
        {
            ViewBag.antrenor_id = antrenor_id;
            ViewBag.kullanici_id = kullanici_id;
            List<kullanici> kullaniciListesi = _context.Kullanicilar.Where(k => k.kullanici_id == kullanici_id).ToList();
            List<antrenor1> antrenorListesi = _context.Antrenorler.Where(k => k.antrenor_id == antrenor_id).ToList();
            String pictureAntrenor = "";

            foreach (var item in antrenorListesi)
            {
                pictureAntrenor = item.picture;
            }
            ViewBag.pictureAntrenor = pictureAntrenor;
            String pictureKullanici = "";

            foreach (var item in kullaniciListesi)
            {
                pictureKullanici = item.picture;
            }
            ViewBag.pictureKullanici = pictureKullanici;


            List<mesaj> mesajlarim = _context.Mesajlarım
        .Where(m => (m.gonderen_id == antrenor_id && m.durum_kontrol == true && m.alici_id == kullanici_id) || (m.alici_id == antrenor_id && m.durum_kontrol == false && m.gonderen_id == kullanici_id))
        .ToList();
            var model = new MesajViewModel
            {
                Mesajlar = mesajlarim
            };





            return View(model);

        }










        public List<mesaj> TumMesajlariGetir()
        {
            List<mesaj> mesaj = _context.Mesajlarım.ToList();
            return mesaj;
        }
        [HttpPost]
        public IActionResult KullaniciMesajGonder(int kullanici_id, int antrenor_id, string mesaj)
        {

            var yenimesaj = new mesaj
            {


                gonderen_id = kullanici_id,
                text = mesaj,
                alici_id = antrenor_id,
                durum_kontrol = false

            };

            // Veritabanına yeni veriyi ekle
            _context.Mesajlarım.Add(yenimesaj);
            _context.SaveChanges();


            return RedirectToAction("Index3", new { antrenor_id = antrenor_id, kullanici_id = kullanici_id });

        }
        public IActionResult Index1(int kullanici_id)
        {
            List<kullanici> kullanici = _context.Kullanicilar.Where(bp => bp.kullanici_id == kullanici_id).ToList();

            var viewModel = new AntrenorKullaniciViewModel
            {
                KullaniciListesi = kullanici

            };



            return View(viewModel);


        }
        [HttpPost]
        public IActionResult UpdateKullanici(int id, string ad, string soyad, DateTime dogumTarih, bool cinsiyet, string eposta, string telefon, string picture, string sifre, int antrenor_Id, int hedef)
        {


            try
            {
                var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.kullanici_id == id);
                if (kullanici != null)
                {
                    kullanici.kullanici_ad = ad;
                    kullanici.kullanici_soyad = soyad;
                    kullanici.dogum_tarih = dogumTarih;
                    kullanici.cinsiyet = cinsiyet;
                    kullanici.e_posta = eposta;
                    kullanici.telefon = telefon;
                    kullanici.picture = picture ?? "";
                    kullanici.sifre = sifre;
                    kullanici.antrenor_id = antrenor_Id;
                    kullanici.hedef = hedef;

                    _context.SaveChanges();
                }
                else
                {

                    return RedirectToAction("Index", new { id = id });


                }

                return RedirectToAction("Index", new { id = id });
            }
            catch (Exception ex)
            {
                ViewBag.UpdateError = "Güncelleme işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index", new { id = id });
            }
        }
        public IActionResult Index2(int kullanici_id)
        {
            List<guncel_versiyon> versiyon = _context.GuncelVersiyon.Where(bp => bp.kullanici_id == kullanici_id).ToList();
            ViewBag.kullanici_id = kullanici_id;
            var viewModel = new GunceVersiyonViewModel
            {
                GuncelVersiyonlar = versiyon

            };



            return View(viewModel);


        }
        public List<guncel_versiyon> TumVersiyonlariGetir()
        {
            List<guncel_versiyon> versiyonlar = _context.GuncelVersiyon.ToList();
            return versiyonlar;
        }
        [HttpPost]
        public IActionResult Ekle(int kullanici_boy, int kullanici_kilo, int kas_orani, int yag_orani, int yakilan_kalori, int alinan_kalori, int kullanici_id)
        {
            try
            {

                List<guncel_versiyon> versiyon = TumVersiyonlariGetir();
                int idm = 0;
                foreach (var item in versiyon)
                {
                    idm = item.id;

                }
                idm += idm;
                // Yeni veriyi oluştur
                var yeniVersiyon = new guncel_versiyon
                {
                    id = idm,
                    kullanici_id = kullanici_id,
                    kullanici_boy = kullanici_boy,
                    kullanici_kilo = kullanici_kilo,
                    kas_orani = kas_orani,
                    yag_orani = yag_orani,
                    yakilan_kalori = yakilan_kalori,
                    alinan_kalori = alinan_kalori
                };

                // Veritabanına yeni veriyi ekle
                _context.GuncelVersiyon.Add(yeniVersiyon);
                _context.SaveChanges();

                return RedirectToAction("Index", new { id = kullanici_id }); // Ekleme başarılıysa Index sayfasına yönlendir
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajıyla birlikte Index sayfasına geri dön
                ViewBag.UpdateError = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index", new { id = kullanici_id });
            }
        }





    }
}