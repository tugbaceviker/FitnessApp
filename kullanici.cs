using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp1
{
    public class kullanici
    {

        public string kullanici_ad { get; set; }

        public string kullanici_soyad { get; set; }


        public DateTime dogum_tarih { get; set; }

        public bool cinsiyet { get; set; }

        public string e_posta { get; set; }

        public string telefon { get; set; }



        public string picture { get; set; }

        public string sifre { get; set; }


        public int antrenor_id { get; set; }
        [Key]
        public int kullanici_id { get; set; }



        public int hedef { get; set; }
    }
}