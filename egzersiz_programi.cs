using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp1
{
    public class egzersiz_programi
    {
        [Key]
        public int id { get; set; }
        public int kullanici_id { get; set; }

        public int antrenor_id { get; set; }

        public string egzersiz_ad { get; set; }

        public int hedef { get; set; }

        public int set_sayi { get; set; }

        public string video_link { get; set; }

        public DateTime baslama_tarih { get; set; }

        public string program_sure { get; set; }








    }
}