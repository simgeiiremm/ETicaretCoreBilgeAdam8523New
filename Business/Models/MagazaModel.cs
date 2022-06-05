using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class MagazaModel : RecordBase
    {
        [Required]
        [StringLength(200)]
        public string Adi { get; set; }

        public bool SanalMi { get; set; }

        [Range(1, 5, ErrorMessage = "{0} {1} ile {2} arasında olmalıdır!")]
        [DisplayName("Puanı")]
        public byte? Puani { get; set; }

        public string SanalMiDisplay { get; set; }
    }
}
