using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp1
{
    public class beslenme_programi
    {


        [Key]
        public int id { get; set; }

        public int kullanici_id { get; set; }

        public int antrenor_id { get; set; }

        public int hedef { get; set; }

        public string gunluk_ogun { get; set; }

        public int kalori_hedef { get; set; }



    }
}