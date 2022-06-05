using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services
{
    public interface IKullaniciService : IService<KullaniciModel, Kullanici, ETicaretContext>
    {

    }

    public class KullaniciService : IKullaniciService
    {
        public RepoBase<Kullanici, ETicaretContext> Repo { get; set; } = new Repo<Kullanici, ETicaretContext>();

        public Result Add(KullaniciModel model)
        {
            if (Repo.EntityExists(k => k.KullaniciAdi == model.KullaniciAdi))
                return new ErrorResult("Girilen kullanıcı adına sahip kayıt bulunmaktadır!");
            if (Repo.EntityExists(k => k.KullaniciDetayi.Eposta.ToLower() == model.Eposta.ToLower().Trim()))
                return new ErrorResult("Girilen e-postaya sahip kayıt bulunmaktadır!");
            Kullanici entity = new Kullanici()
            {
                AktifMi = model.AktifMi,
                KullaniciAdi = model.KullaniciAdi,
                RolId = model.RolId,
                Sifre = model.Sifre,
                KullaniciDetayi = new KullaniciDetayi()
                {
                    Adres = model.Adres,
                    Cinsiyet = model.Cinsiyet,
                    Eposta = model.Eposta,
                    SehirId = model.SehirId.Value,
                    UlkeId = model.UlkeId.Value
                }
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<KullaniciModel> Query()
        {
            return Repo.Query("Rol").Select(e => new KullaniciModel()
            {
                AktifMi = e.AktifMi,
                Id = e.Id,
                KullaniciAdi = e.KullaniciAdi,
                RolId = e.RolId,
                Sifre = e.Sifre,

                RolAdiDisplay = e.Rol.Adi
            });
        }

        public Result Update(KullaniciModel model)
        {
            throw new NotImplementedException();
        }
    }

}
