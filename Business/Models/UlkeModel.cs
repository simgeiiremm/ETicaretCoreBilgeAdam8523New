using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UlkeModel : RecordBase
    {
        [Required(ErrorMessage = "{0} gereklidir!")]
        [StringLength(200, ErrorMessage = "{0} maksimum {1} karakter olmalıdır!")]
        [DisplayName("Ülke Adı")]
        public string Adi { get; set; }
    }
}
