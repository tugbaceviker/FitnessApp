using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp1
{
    public class guncel_versiyon
    {
        [Key]
        public int id { get; set; }

        public int kullanici_id { get; set; }

        public int kullanici_boy { get; set; }

        public int kullanici_kilo { get; set; }

        public int kas_orani { get; set; }

        public int yag_orani { get; set; }

        public int yakilan_kalori { get; set; }

        public int alinan_kalori { get; set; }






    }
}