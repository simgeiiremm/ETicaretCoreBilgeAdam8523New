using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Enums;
using Business.Models;

namespace Business.Services
{
    public interface IHesapService
    {
        Result<KullaniciModel> Giris(KullaniciGirisModel model);
        Result Kayit(KullaniciKayitModel model);
    }

    public class HesapService : IHesapService
    {
        private readonly IKullaniciService _kullaniciService;

        public HesapService(IKullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        public Result<KullaniciModel> Giris(KullaniciGirisModel model)
        {
            KullaniciModel kullanici = _kullaniciService.Query().SingleOrDefault(k => k.KullaniciAdi == model.KullaniciAdi && k.Sifre == model.Sifre && k.AktifMi);
            if (kullanici == null)
                return new ErrorResult<KullaniciModel>("Geçersiz kullanıcı adı ve şifre!");
            return new SuccessResult<KullaniciModel>(kullanici);
        }

        public Result Kayit(KullaniciKayitModel model)
        {
            KullaniciModel kullanici = new KullaniciModel()
            {
                AktifMi = true,
                KullaniciAdi = model.KullaniciAdi,
                RolId = (int)Roller.Kullanıcı,
                Sifre = model.Sifre,
                Adres = model.Adres.Trim(),
                Eposta = model.Eposta,
                UlkeId = model.UlkeId,
                SehirId = model.SehirId,
                Cinsiyet = model.Cinsiyet
            };
            return _kullaniciService.Add(kullanici);
        }
    }
}
