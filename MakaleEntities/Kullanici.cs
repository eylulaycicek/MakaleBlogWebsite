using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities
{
    [Table("Kullanici")]
    public class Kullanici:BaseClass
    {
        [StringLength(30)]
        public string Adi { get; set; }

        [StringLength(30)]
        public string Soyad { get; set; }

        [Required,StringLength(30)]
        public string KullaniciAdi { get; set; }

        [Required, StringLength(200)]
        public string Email { get; set; }

        [Required, StringLength(20)]
        public string Sifre { get; set; }
        public bool Aktif { get; set; }

        [Required]
        public Guid AktifGuid { get; set; }
        public bool Admin { get; set; }
        public virtual List<Makale> Makaleler { get; set; } 
        public virtual List<Yorum> Yorumlar { get; set; }
        public virtual List<Begeni> Begeniler { get; set; }
    }
}
