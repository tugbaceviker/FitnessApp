using DataAccessLayer.Concrete;
using FitnessApp1.Models;
using FitnessApp1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp1.Controllers
{
    public class AdminPaneliController : Controller
    {
        private readonly Context _context;

        public AdminPaneliController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Tüm kullanıcıları ve antrenörleri ayrı listelere al
            var tumKullanicilar = _context.Kullanicilar.ToList();
            var tumAntrenorler = _context.Antrenorler.ToList();

            // ViewModel'e verileri aktar
            var viewModel = new AdminPanelViewModel
            {
                Kullanicilar = tumKullanicilar,
                Antrenorler = tumAntrenorler
            };

            return View(viewModel);
        }
        public IActionResult Index1(int antrenorid)
        {

            ViewBag.Antrenorid = antrenorid;
            List<antrenor1> antrenorler = _context.Antrenorler.Where(bp => bp.antrenor_id == antrenorid).ToList();
            List<kullanici> kullanicilar = _context.Kullanicilar.Where(bp => bp.antrenor_id == antrenorid).ToList();
           
            String pictureAntrenor = "";

            foreach (var item in antrenorler)
            {
                pictureAntrenor = item.picture;
            }
            ViewBag.pictureAntrenor = pictureAntrenor;
            String pictureKullanici = "";

            foreach (var item in kullanicilar)
            {
                pictureKullanici = item.picture;
            }
            ViewBag.pictureKullanici = pictureKullanici;


            var model = new AdminPanelViewModel
            {

                Antrenorler = antrenorler,
                Kullanicilar = kullanicilar
            };



            return View(model);
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

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajıyla birlikte Index sayfasına geri dön
                ViewBag.UpdateError = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index", new { id = kullanici_id });
            }
        }
        public List<kullanici> TumKullanicilariiGetir()
        {
            List<kullanici> kullanicilar = _context.Kullanicilar.ToList();
            return kullanicilar;
        }
        public List<antrenor1> TumAntrenorleriGetir()
        {
            List<antrenor1> antrenorler = _context.Antrenorler.ToList();
            return antrenorler;
        }
        [HttpPost]
        public IActionResult Delete(int kullaniciId)
        {
            var kullanici = _context.Kullanicilar.FirstOrDefault(k => k.kullanici_id == kullaniciId);
            if (kullanici != null)
            {
                _context.Kullanicilar.Remove(kullanici);
                _context.SaveChanges();
                // Silme işlemi başarılı oldu, gerekli yönlendirme yapılabilir veya bir mesaj gösterilebilir.
                return RedirectToAction("Index"); // Örneğin, kullanıcıları listeleme sayfasına geri dönebilirsin.
            }
            else
            {
                // Kullanıcı bulunamadı veya silinemediğinde bir hata mesajı gösterebilirsin.
                return NotFound(); // Örneğin, 404 sayfasına yönlendirme yapabilirsin.
            }
        }
        [HttpPost]
        public IActionResult DeleteAntrenor(int antrenorId)
        {
            var antrenor = _context.Antrenorler.FirstOrDefault(a => a.antrenor_id == antrenorId);
            if (antrenor != null)
            {
                // Kullanıcıları bul ve ilişkilendirilmiş antrenör_id'leri güncelle
                var usersToUpdate = _context.Kullanicilar.Where(k => k.antrenor_id == antrenorId).ToList();
                foreach (var user in usersToUpdate)
                {
                    user.antrenor_id = 0; // veya başka bir değer atayabilirsin, örneğin null
                }

                _context.Antrenorler.Remove(antrenor);
                _context.SaveChanges();
                // Silme işlemi başarılı oldu, gerekli yönlendirme yapılabilir veya bir mesaj gösterilebilir.
                return RedirectToAction("Index"); // Örneğin, antrenörleri listeleme sayfasına geri dönebilirsin.
            }
            else
            {
                // Antrenör bulunamadı veya silinemediğinde bir hata mesajı gösterebilirsin.
                return NotFound(); // Örneğin, 404 sayfasına yönlendirme yapabilirsin.
            }
        }
        [HttpPost]
        public IActionResult DeleteBeslenme(int id)
        {
            var beslenme = _context.BeslenmeProgramlari.FirstOrDefault(k => k.id == id);
            if (beslenme != null)
            {
                _context.BeslenmeProgramlari.Remove(beslenme);
                _context.SaveChanges();
                // Silme işlemi başarılı oldu, gerekli yönlendirme yapılabilir veya bir mesaj gösterilebilir.
                return RedirectToAction("Index"); // Örneğin, kullanıcıları listeleme sayfasına geri dönebilirsin.
            }
            else
            {
                // Kullanıcı bulunamadı veya silinemediğinde bir hata mesajı gösterebilirsin.
                return NotFound(); // Örneğin, 404 sayfasına yönlendirme yapabilirsin.
            }
        }
        [HttpPost]
        public IActionResult DeleteEgzersiz(int id)
        {
            var egzersiz = _context.EgzersizProgramlari.FirstOrDefault(k => k.id == id);
            if (egzersiz != null)
            {
                _context.EgzersizProgramlari.Remove(egzersiz);
                _context.SaveChanges();
                // Silme işlemi başarılı oldu, gerekli yönlendirme yapılabilir veya bir mesaj gösterilebilir.
                return RedirectToAction("Index"); // Örneğin, kullanıcıları listeleme sayfasına geri dönebilirsin.
            }
            else
            {
                // Kullanıcı bulunamadı veya silinemediğinde bir hata mesajı gösterebilirsin.
                return NotFound(); // Örneğin, 404 sayfasına yönlendirme yapabilirsin.
            }
        }
        [HttpPost]
        public IActionResult DeleteVersiyon(int id)
        {
            var versiyon = _context.GuncelVersiyon.FirstOrDefault(k => k.id == id);
            if (versiyon != null)
            {
                _context.GuncelVersiyon.Remove(versiyon);
                _context.SaveChanges();
                // Silme işlemi başarılı oldu, gerekli yönlendirme yapılabilir veya bir mesaj gösterilebilir.
                return RedirectToAction("Index"); // Örneğin, kullanıcıları listeleme sayfasına geri dönebilirsin.
            }
            else
            {
                // Kullanıcı bulunamadı veya silinemediğinde bir hata mesajı gösterebilirsin.
                return NotFound(); // Örneğin, 404 sayfasına yönlendirme yapabilirsin.
            }
        }


        public IActionResult Index2(int kullaniciid)
        {
            List<kullanici> tumKullanicilar = TumKullanicilariiGetir();
            int antrenor = 0;
            string picture = "";
            foreach (var item in tumKullanicilar)
            {
                if (item.kullanici_id == kullaniciid)
                {
                    picture = item.picture;
                    antrenor = item.antrenor_id;
                }
                
            }

            ViewBag.picture = picture;

            ViewBag.kullanici_id = kullaniciid;
            ViewBag.antrenor_id = antrenor;
            List<kullanici> kullanicilar = _context.Kullanicilar.Where(bp => bp.kullanici_id == kullaniciid).ToList();
            List<beslenme_programi> beslenmeprogrami = _context.BeslenmeProgramlari.Where(bp => bp.kullanici_id == kullaniciid).ToList();
            List<egzersiz_programi> egzersizprogrami = _context.EgzersizProgramlari.Where(bp => bp.kullanici_id == kullaniciid).ToList();
            List<antrenor1> antrenorler = _context.Antrenorler.Where(bp => bp.antrenor_id != antrenor).ToList();
            List<guncel_versiyon> guncelversiyonlar = _context.GuncelVersiyon.Where(bp => bp.kullanici_id == kullaniciid).ToList();
            
            var model1 = new AdminTumTablolarViewModel
            {

                Antrenorler = antrenorler,
                Kullanicilar = kullanicilar,
                BeslenmeProgramlari = beslenmeprogrami,
                EgzersizProgramlari = egzersizprogrami,
                GuncelVersiyonlari = guncelversiyonlar


            };



            return View(model1);
        }
        [HttpPost]
        public IActionResult KayitKontrol(string kullaniciAd, string kullaniciSoyad, DateTime dogumTarih, bool cinsiyet, string kullaniciEposta, string telefon, string picture, string sifre, int hedef)
        {
            try
            {
                List<kullanici> tumKullanicilar = TumKullanicilariiGetir();
                int id = 0;
                foreach (var item in tumKullanicilar)
                {
                    id = item.kullanici_id;

                }
                id += id;
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
                    kullanici_id = id, // Otomatik olarak atanacak
                    hedef = hedef
                };


                _context.Kullanicilar.Add(yeniKullanici);
                _context.SaveChanges();

                // Başarılı kayıt durumunda, giriş sayfasına yönlendir
                return RedirectToAction("Index", "AdminPaneli");
            }
            catch (Exception ex)
            {
                // Hata durumunda, aynı sayfaya hata mesajıyla birlikte yönlendir
                ViewBag.KayitError = "Kayıt işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return View("Index");
            }
        }
        [HttpPost]
        public IActionResult KayitKontrol1(string antrenorad, string antrenorsoyad, string eposta, string telefon, string picture, int uzmanlik)
        {
            try
            {
                // Mevcut antrenörleri al
                List<antrenor1> tumantrenorler = TumAntrenorleriGetir();
                int id = 0;
                foreach (var item in tumantrenorler)
                {
                    id = item.antrenor_id;
                }
                id += id;
                // Yeni antrenör oluşturma işlemi
                antrenor1 yeniantrenor = new antrenor1
                {
                    antrenor_ad = antrenorad,
                    antrenor_soyad = antrenorsoyad,
                    e_posta = eposta,
                    telefon = telefon,

                    picture = picture ?? "",
                    antrenor_id = id,

                    uzmanlik = uzmanlik
                };

                _context.Antrenorler.Add(yeniantrenor);
                _context.SaveChanges();

                // Başarılı kayıt durumunda, giriş sayfasına yönlendir
                return RedirectToAction("Index", "AdminPaneli");
            }
            catch (Exception ex)
            {
                // Hata durumunda, aynı sayfaya hata mesajıyla birlikte yönlendir
                ViewBag.KayitError = "Kayıt işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return View("Index");
            }
        }
        [HttpPost]
        public IActionResult UpdateAntrenor(string ad, string soyad, string eposta, string telefon, int uzmanlik, int id,string picture)
        {
            try
            {
                var antrenor = _context.Antrenorler.FirstOrDefault(a => a.antrenor_id == id);
                if (antrenor != null)
                {
                    antrenor.antrenor_ad = ad;
                    antrenor.antrenor_soyad = soyad;
                    antrenor.e_posta = eposta;
                    antrenor.telefon = telefon;
                    antrenor.uzmanlik = uzmanlik;
                    antrenor.picture = picture??"";

                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.UpdateError = "Güncelleme işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index");
            }
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

                    return RedirectToAction("Index");

                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.UpdateError = "Güncelleme işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdateBeslenmeProgrami(int hedef, string gunluk_ogun, int kalori_hedef, int id)
        {
            try
            {
                string updateQuery = $"UPDATE beslenme_programi SET  hedef = {hedef}, gunluk_ogun = '{gunluk_ogun}', kalori_hedef = {kalori_hedef} WHERE id = {id}";

                _context.Database.ExecuteSqlRaw(updateQuery);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.UpdateError = "Güncelleme işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult UpdateEgzersizProgrami(int id_egzersiz, int hedef, int set_sayi, string video_link, string program_sure, string egzersiz_ad, DateTime baslama_tarih)
        {
            try
            {
                string updateQuery = $"UPDATE egzersiz_programi SET hedef = {hedef}, egzersiz_ad = '{egzersiz_ad}',baslama_tarih = '{baslama_tarih}', set_sayi = {set_sayi}, video_link = '{video_link}', program_sure = '{program_sure}' WHERE id = {id_egzersiz}";

                _context.Database.ExecuteSqlRaw(updateQuery);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.UpdateError = "Güncelleme işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index");
            }
        }
        public List<guncel_versiyon> TumVersiyonlariGetir()
        {
            List<guncel_versiyon> versiyonlar = _context.GuncelVersiyon.ToList();
            return versiyonlar;
        }
        [HttpPost]
        public IActionResult VersiyonEkle(int kullanici_boy, int kullanici_kilo, int kas_orani, int yag_orani, int yakilan_kalori, int alinan_kalori, int kullanici_id)
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

                return RedirectToAction("Index"); // Ekleme başarılıysa Index sayfasına yönlendir
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajıyla birlikte Index sayfasına geri dön
                ViewBag.UpdateError = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index", new { id = kullanici_id });
            }
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

                return RedirectToAction("Index"); // Ekleme başarılıysa Index sayfasına yönlendir
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajıyla birlikte Index sayfasına geri dön
                ViewBag.UpdateError = "Bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index", new { id = kullanici_id });
            }
        }
        [HttpPost]
        public IActionResult UpdateGuncelVersiyon(int id_versiyon, int kullanici_boy, int kullanici_kilo, int kas_orani, int yag_orani, int yakilan_kalori, int alinan_kalori)
        {
            try
            {
                string updateQuery = $"UPDATE guncel_versiyon SET kullanici_boy = {kullanici_boy}, kullanici_kilo = {kullanici_kilo}, kas_orani = {kas_orani}, yag_orani = {yag_orani}, yakilan_kalori = {yakilan_kalori}, alinan_kalori = {alinan_kalori} WHERE id = {id_versiyon} ";

                _context.Database.ExecuteSqlRaw(updateQuery);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.UpdateError = "Güncelleme işlemi sırasında bir hata oluştu. Lütfen tekrar deneyin.";
                return RedirectToAction("Index");
            }
        }

















    }
}