using DataAccessLayer.Concrete;
using FitnessApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace FitnessApp1.Controllers
{
    public class AntrenorAnaPanelController : Controller
    {
        private readonly Context _context;

        public AntrenorAnaPanelController(Context context)
        {
            _context = context;
        }

        public IActionResult Index(int antrenorId)
        {
            ViewBag.Id = antrenorId;
            // İlgili antrenör ID'sine sahip kullanıcıları getir
            List<kullanici> kullaniciListesi = _context.Kullanicilar.Where(k => k.antrenor_id == antrenorId).ToList();
            List<antrenor1> antrenorListesi = _context.Antrenorler.Where(k => k.antrenor_id == antrenorId).ToList();
            String picture = "";

            foreach (var item in antrenorListesi)
            {
                picture = item.picture;
            }
            ViewBag.picture = picture;

            // Model oluştur ve kullanıcıları bu modele ata
            var model = new AntrenorKullaniciViewModel
            {
                KullaniciListesi = kullaniciListesi
            };

            // View'e modeli gönder
            return View(model);
        }
        public IActionResult Index3(int antrenor_id, int kullanici_id)
        {
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
            ViewBag.antrenor_id = antrenor_id;
            ViewBag.kullanici_id = kullanici_id;


            List<mesaj> mesajlarim = _context.Mesajlarım
        .Where(m => (m.gonderen_id == antrenor_id && m.durum_kontrol == true && m.alici_id == kullanici_id) || (m.alici_id == antrenor_id && m.durum_kontrol == false && m.gonderen_id == kullanici_id))
        .ToList();
            var model = new MesajViewModel
            {
                Mesajlar = mesajlarim
            };





            return View(model);
        }


        public IActionResult Index1(int kullanici_id, int antrenor_id)
        {
            ViewBag.id = kullanici_id;
            ViewBag.antrenor_id = antrenor_id;
            List<beslenme_programi> beslenmeprogrami = _context.BeslenmeProgramlari
                .Where(k => k.antrenor_id == antrenor_id && k.kullanici_id == kullanici_id)
                .ToList();
            List<egzersiz_programi> egzersizprogrami = _context.EgzersizProgramlari
     .Where(k => k.antrenor_id == antrenor_id && k.kullanici_id == kullanici_id)
     .ToList();

            List<guncel_versiyon> guncelverisyon = _context.GuncelVersiyon
                .Where(k => k.kullanici_id == kullanici_id)
                .ToList();

            var model = new AntrenorKullaniciAnaTablolar
            {
                EgzersizProgramlari = egzersizprogrami,
                BeslenmeProgramlari = beslenmeprogrami,
                GuncelVersiyonlari = guncelverisyon
            };




            return View(model);
        }
        public IActionResult Index2(int antrenor_id)
        {

            List<antrenor1> antrenor = _context.Antrenorler.Where(k => k.antrenor_id == antrenor_id).ToList();

            var model = new AntrenorViewModel
            {
                Antrenorler = antrenor

            };




            return View(model);
        }

        public IActionResult EgzersizDelete(int kullanici_id, int antrenor_id, int id)
        {
            string deleteQuery = "DELETE FROM egzersiz_programi WHERE id =@id ";

            _context.Database.ExecuteSqlRaw(deleteQuery,

                new SqlParameter("@id", id));

            return RedirectToAction("Index1", new { kullanici_id = kullanici_id, antrenor_id = antrenor_id });
        }

        public List<mesaj> TumMesajlariGetir()
        {
            List<mesaj> mesaj = _context.Mesajlarım.ToList();
            return mesaj;
        }

        [HttpPost]
        public IActionResult AntrenorMesajGonder(int kullanici_id, int antrenor_id, string mesaj)
        {

            try
            {
                string sql = "INSERT INTO mesaj (gonderen_id, alici_id, text, durum_kontrol) VALUES ({0}, {1}, {2}, {3})";
                _context.Database.ExecuteSqlRaw(sql, antrenor_id, kullanici_id, mesaj, true);

                return RedirectToAction("Index3", new { antrenor_id = antrenor_id, kullanici_id = kullanici_id });
            }
            catch (Exception ex)
            {
                ViewBag.UpdateError = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index3", new { antrenor_id = antrenor_id, kullanici_id = kullanici_id });
            }
        }

        public IActionResult BeslenmeDelete(int kullanici_id, int antrenor_id, int id)
        {
            string deleteQuery = "DELETE FROM beslenme_programi WHERE id =@id ";

            _context.Database.ExecuteSqlRaw(deleteQuery,

                new SqlParameter("@id", id));

            return RedirectToAction("Index1", new { kullanici_id = kullanici_id, antrenor_id = antrenor_id });
        }

        public List<beslenme_programi> TumBeslenmeleriGetir()
        {
            List<beslenme_programi> beslenmeler = _context.BeslenmeProgramlari.ToList();
            return beslenmeler;
        }
        [HttpPost]
        public IActionResult BeslenmeEkle(int kullanici_id, int antrenor_id, int hedef, int kalori_hedef, string gunluk_ogun)
        {
            try
            {

                List<beslenme_programi> beslenmelerler = TumBeslenmeleriGetir();
                int idm = 0;
                foreach (var item in beslenmelerler)
                {
                    idm = item.id;

                }
                idm += idm;
                // Yeni veriyi oluştur
                var yenibeslenme = new beslenme_programi
                {
                    id = idm,
                    kullanici_id = kullanici_id,
                    antrenor_id = antrenor_id,
                    hedef = hedef,
                    kalori_hedef = kalori_hedef,
                    gunluk_ogun = gunluk_ogun
                };

                // Veritabanına yeni veriyi ekle
                _context.BeslenmeProgramlari.Add(yenibeslenme);
                _context.SaveChanges();

                return RedirectToAction("Index1", new { kullanici_id = kullanici_id, antrenor_id = antrenor_id }); // Ekleme başarılıysa Index sayfasına yönlendir
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajıyla birlikte Index sayfasına geri dön
                ViewBag.UpdateError = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index", new { id = kullanici_id });
            }
        }


        [HttpPost]
        public IActionResult Guncelle(string ad, string soyad, string eposta, string telefon, int uzmanlik, int a_id, string picture)
        {
            try
            {
                var antrenor = _context.Antrenorler.FirstOrDefault(a => a.antrenor_id == a_id);
                if (antrenor != null)
                {
                    antrenor.antrenor_ad = ad;
                    antrenor.antrenor_soyad = soyad;
                    antrenor.e_posta = eposta;
                    antrenor.telefon = telefon;
                    antrenor.uzmanlik = uzmanlik;
                    antrenor.picture = picture;

                    _context.SaveChanges();
                }

                return RedirectToAction("Index", new { antrenorId = a_id });
            }
            catch (Exception ex)
            {
                ViewBag.UpdateError = "Güncelleme işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index");
            }
        }
        public List<egzersiz_programi> TumEgzersizleriGetir()
        {
            List<egzersiz_programi> egzersizler = _context.EgzersizProgramlari.ToList();
            return egzersizler;
        }
        [HttpPost]
        public IActionResult EgzersizEkle(int kullanici_id, int antrenor_id, int hedef, int set_sayi, string video_link, DateTime baslama_tarih, string program_sure, string egzersiz_ad)
        {
            try
            {

                List<egzersiz_programi> egzersizler = TumEgzersizleriGetir();
                int idm = 0;
                foreach (var item in egzersizler)
                {
                    idm = item.id;

                }
                idm += idm;
                // Yeni veriyi oluştur
                var yeniEgzersiz = new egzersiz_programi
                {
                    id = idm,
                    kullanici_id = kullanici_id,
                    antrenor_id = antrenor_id,
                    hedef = hedef,
                    set_sayi = set_sayi,
                    video_link = video_link,
                    baslama_tarih = baslama_tarih,
                    program_sure = program_sure,
                    egzersiz_ad = egzersiz_ad
                };

                // Veritabanına yeni veriyi ekle
                _context.EgzersizProgramlari.Add(yeniEgzersiz);
                _context.SaveChanges();

                return RedirectToAction("Index1", new { kullanici_id = kullanici_id, antrenor_id = antrenor_id }); // Ekleme başarılıysa Index sayfasına yönlendir
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