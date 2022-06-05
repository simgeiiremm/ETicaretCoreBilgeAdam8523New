using AppCore.Business.Models.Results;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Repositories.Bases;

namespace Business.Services
{
    public class KategoriService : IKategoriService
    {
        public RepoBase<Kategori, ETicaretContext> Repo { get; set; } = new Repo<Kategori, ETicaretContext>();

        //public KategoriService(KategoriRepoBase kategoriRepo)
        //{
        //    Repo = kategoriRepo;
        //}
        // program.cs'de IoC container'da yönetilmediği için burada enjekte etmeye gerek kalmadı

        public Result Add(KategoriModel model)
        {
            //Kategori existingEntity = Repo.Query().SingleOrDefault(kategori => kategori.Adi.ToUpper() == model.Adi.ToUpper().Trim());
            //Kategori existingEntity = Repo.Query(kategori => kategori.Adi.ToLower() == model.Adi.ToLower().Trim()).SingleOrDefault();
            //if (existingEntity != null)
            //    return new ErrorResult("Girdiğiniz kategori adına sahip kayıt bulunmaktadır!");
            if (Repo.Query().Any(kategori => kategori.Adi.ToUpper() == model.Adi.ToUpper().Trim()))
                return new ErrorResult("Girdiğiniz kategori adına sahip kayıt bulunamktadır!");

            Kategori entity = new Kategori()
            {
                Adi = model.Adi.Trim(),
                //Aciklamasi = string.IsNullOrWhiteSpace(model.Aciklamasi) ? null : model.Aciklamasi.Trim()
                Aciklamasi = model.Aciklamasi?.Trim()
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            //Repo.Delete(k => k.Id == id);
            //Kategori entity = Repo.Query("Urunler").SingleOrDefault(k => k.Id == id);
            Kategori entity = Repo.Query(k => k.Id == id, "Urunler").SingleOrDefault();
            if (entity.Urunler != null && entity.Urunler.Count > 0)
            {
                return new ErrorResult("Kategori silinemez çünkü ilişkili ürünler bulunmaktadır!");
            }
            Repo.Delete(entity);
            return new SuccessResult("Kategori başarıyla silindi.");
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<KategoriModel> Query()
        {
            IQueryable<KategoriModel> query = Repo.Query("Urunler").OrderBy(k => k.Adi).Select(k => new KategoriModel()
            {
                Id = k.Id,
                Adi = k.Adi,
                Aciklamasi = k.Aciklamasi,
                UrunSayisiDisplay = k.Urunler.Count
            });
            return query;
        }

        public Result Update(KategoriModel model)
        {
            if (Repo.Query().Any(kategori => kategori.Adi.ToUpper() == model.Adi.ToUpper().Trim() && kategori.Id != model.Id))
                return new ErrorResult("Girdiğiniz kategori adına sahip kayıt bulunamktadır!");
            Kategori entity = Repo.Query().SingleOrDefault(kategori => kategori.Id == model.Id);
            entity.Adi = model.Adi.Trim();
            entity.Aciklamasi = model.Aciklamasi?.Trim();
            Repo.Update(entity);
            return new SuccessResult("Kategori başarıyla güncellendi.");
        }
    }
}
