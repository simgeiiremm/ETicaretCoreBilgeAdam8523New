using System.ComponentModel;

namespace Business.Models.Filters
{
    public class UrunFilterModel
    {
        [DisplayName("Ürün Adı")]
        public string UrunAdi { get; set; }
        [DisplayName("Birim Fiyatı")]
        public double? BirimFiyatiBaslangic { get; set; }
        public double? BirimFiyatiBitis { get; set; }
        [DisplayName("Stok Miktarı")]
        public int? StokMiktariBaslangic { get; set; }
        public int? StokMiktariBitis { get; set; }

        [DisplayName("Son Kullanma Tarihi")]
        public DateTime? SonKullanmaTarihiBaslangic { get; set; }
        public DateTime? SonKullanmaTarihiBitis { get; set; }

        [DisplayName("Kategori")]
        public int? KategoriId { get; set; }

        [DisplayName("Mağaza")]
        public List<int> MagazaIdleri { get; set; }

    }
}
