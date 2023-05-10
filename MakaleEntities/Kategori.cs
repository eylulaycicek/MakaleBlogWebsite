using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleEntities
{
    [Table("Kategori")]
    public class Kategori:BaseClass
    {
        [Required,StringLength(50),DisplayName("Kategori")]
        public string Baslik { get; set; }

        [StringLength(150)]
        public string Aciklama { get; set; }

        public virtual List<Makale> Makaleler { get; set; }

        public Kategori()
        {
            Makaleler = new List<Makale>();  // null olduğu için bir tanımlama yaptık
        }
    }
}
