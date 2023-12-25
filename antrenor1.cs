using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp1
{
    public class antrenor1
    {
        public string antrenor_ad { get; set; }

        public string antrenor_soyad { get; set; }

        public string e_posta { get; set; }

        public string telefon { get; set; }

        public bool cinsiyet { get; set; }

        public string picture { get; set; }
        [Key]
        public int antrenor_id { get; set; }

        public string sifre { get; set; }

        public int uzmanlik { get; set; }










    }
}