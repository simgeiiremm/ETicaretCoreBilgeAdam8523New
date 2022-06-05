using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class SehirModel : RecordBase
    {
        [Required(ErrorMessage = "{0} gereklidir!")]
        [StringLength(200, ErrorMessage = "{0} maksimum {1} karakter olmalıdır!")]
        [DisplayName("Şehir Adı")]
        public string Adi { get; set; }

        public int UlkeId { get; set; }

        public string UlkeAdiDisplay { get; set; }
    }
}
