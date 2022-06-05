using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using Business.Models.Filters;
using DataAccess.Contexts;
using DataAccess.Entities;
using System.Globalization;

namespace Business.Services
{
    public interface IUrunService :IService<UrunModel, Urun, ETicaretContext>
    {
        Result<List<UrunModel>> List(UrunFilterModel filtre);
    }

    public class UrunService : IUrunService
    {
        ETicaretContext _dbContext;
        public RepoBase<Urun, ETicaretContext> Repo { get; set; }
        private RepoBase<UrunMagaza, ETicaretContext> _urunMagazaRepo;
        RepoBase<Magaza, ETicaretContext> _magazaRepo;
        RepoBase<Kategori, ETicaretContext> _kategoriRepo;

        public UrunService()
        {
            _dbContext = new ETicaretContext();
            Repo = new Repo<Urun, ETicaretContext>(_dbContext);
            _urunMagazaRepo = new Repo<UrunMagaza, ETicaretContext>(_dbContext);
            _magazaRepo = new Repo<Magaza, ETicaretContext>(_dbContext);
            _kategoriRepo = new Repo<Kategori, ETicaretContext>(_dbContext);
        }

        public Result Add(UrunModel model)
        {
            if (Repo.Query().Any(u => u.Adi.ToLower() == model.Adi.ToLower().Trim()))
                return new ErrorResult("Belirtilen ürün adına sahip kayıt bulunmaktadır!");
            if (model.SonKullanmaTarihi.HasValue && model.SonKullanmaTarihi.Value < DateTime.Today)
                return new ErrorResult("Son kullanma tarihi bugün veya daha sonrası olmalıdır!");
            Urun entity = new Urun()
            {
                Aciklamasi = model.Aciklamasi?.Trim(),
                Adi = model.Adi.Trim(),
                BirimFiyati = model.BirimFiyati.Value,
                KategoriId = model.KategoriId.Value,
                SonKullanmaTarihi = model.SonKullanmaTarihi,
                StokMiktari = model.StokMiktari.Value,
                UrunMagazalar = model.MagazaIdleri.Select(magazaId => new UrunMagaza()
                {
                    MagazaId = magazaId
                }).ToList()
            };

            // 1. yöntem:
            //if (model.MagazaIdleri != null && model.MagazaIdleri.Count > 0)
            //{
            //    entity.UrunMagazalar = new List<UrunMagaza>();
            //    foreach (var magazaId in model.MagazaIdleri)
            //    {
            //        entity.UrunMagazalar.Add(new UrunMagaza()
            //        {
            //            MagazaId = magazaId
            //        });
            //    }
            //}

            Repo.Add(entity);
            return new SuccessResult("Ürün başarıyla eklendi.");
        }

        public Result Delete(int id)
        {
            var urun = Repo.Query("UrunMagazalar").SingleOrDefault(u => u.Id == id);
            if (urun.UrunMagazalar != null && urun.UrunMagazalar.Count > 0)
            {
                foreach (var urunMagaza in urun.UrunMagazalar)
                {
                    _urunMagazaRepo.Delete(urunMagaza, false);
                }
                _urunMagazaRepo.Save();
            }
            Repo.Delete(u => u.Id == id);
            return new SuccessResult("Ürün başarıyla silindi!");
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<UrunModel> Query()
        {
            // AutoMapper
            //return Repo.Query("Kategori").OrderBy(u => u.Adi).Select(u => new UrunModel()
            //{
            //    Id = u.Id,
            //    Aciklamasi = u.Aciklamasi,
            //    Adi = u.Adi,
            //    BirimFiyati = u.BirimFiyati,
            //    KategoriId = u.KategoriId,
            //    SonKullanmaTarihi = u.SonKullanmaTarihi,
            //    StokMiktari = u.StokMiktari,

            //    BirimFiyatiDisplay = u.BirimFiyati.ToString("C2", new CultureInfo("tr-TR")), // "en-US"

            //    SonKullanmaTarihiDisplay = u.SonKullanmaTarihi.HasValue ? u.SonKullanmaTarihi.Value.ToString("yyyy-MM-dd") : "",

            //    KategoriAdiDisplay = u.Kategori.Adi
            //});

            var urunQuery = Repo.Query();
            var urunMagazaQuery = _urunMagazaRepo.Query();
            var magazaQuery = _magazaRepo.Query();
            var kategoriQuery = _kategoriRepo.Query();

            // select u.Adi [Ürün Adı], ... from Urunler u inner join UrunMagazalar um on u.Id = um.UrunId
            // inner join Magazalar m on um.MagazaId = m.Id inner join Kategoriler k on u.KategoriId = k.Id

            //innerjoin örneği
            //var query = from u in urunQuery
            //            join um in urunMagazaQuery
            //            on u.Id equals um.UrunId
            //            join m in magazaQuery
            //            on um.MagazaId equals m.Id
            //            join k in kategoriQuery
            //            on u.KategoriId equals k.Id
            //            select new UrunModel()
            //            {
            //                Id = u.Id,
            //                Aciklamasi = u.Aciklamasi,
            //                Adi = u.Adi,
            //                BirimFiyati = u.BirimFiyati,
            //                KategoriId = u.KategoriId,
            //                SonKullanmaTarihi = u.SonKullanmaTarihi,
            //                StokMiktari = u.StokMiktari,

            //                BirimFiyatiDisplay = u.BirimFiyati.ToString("C2", new CultureInfo("tr-TR")),
            //                SonKullanmaTarihiDisplay = u.SonKullanmaTarihi.HasValue ? u.SonKullanmaTarihi.Value.ToString("yyyy-MM-dd") : "",
            //                KategoriAdiDisplay = k.Adi,
            //                MagazaAdiDisplay = m.Adi
            //            };

            //Left-outer join
            var query = from urun in urunQuery
                        join urunMagaza in urunMagazaQuery
                        on urun.Id equals urunMagaza.UrunId into urunMagazalar
                        from subUrunMagazalar in urunMagazalar.DefaultIfEmpty()
                        join magaza in magazaQuery
                        on subUrunMagazalar.MagazaId equals magaza.Id into magazalar
                        from subMagazalar in magazalar.DefaultIfEmpty()
                        join kategori in kategoriQuery
                        on urun.KategoriId equals kategori.Id into kategoriler
                        from subKategoriler in kategoriler.DefaultIfEmpty()
                        select new UrunModel()
                        {
                            Id = urun.Id,
                            Aciklamasi = urun.Aciklamasi,
                            Adi = urun.Adi,
                            BirimFiyati = urun.BirimFiyati,
                            KategoriId = urun.KategoriId,
                            SonKullanmaTarihi = urun.SonKullanmaTarihi,
                            StokMiktari = urun.StokMiktari,

                            BirimFiyatiDisplay = urun.BirimFiyati.ToString("C2"),
                            SonKullanmaTarihiDisplay = urun.SonKullanmaTarihi.HasValue ? urun.SonKullanmaTarihi.Value.ToString("yyyy-MM-dd") : "",
                            KategoriAdiDisplay = subKategoriler.Adi,
                            MagazaAdiDisplay = subMagazalar.Adi
                        };
            return query;
        }

        public Result Update(UrunModel model)
        {
            if (Repo.Query().Any(u => u.Adi.ToLower() == model.Adi.ToLower().Trim() && u.Id != model.Id))
                return new ErrorResult("Belirtilen ürün adına sahip kayıt bulunmaktadır!");
            if (model.SonKullanmaTarihi.HasValue && model.SonKullanmaTarihi.Value < DateTime.Today)
                return new ErrorResult("Son kullanma tarihi bugün veya daha sonrası olmalıdır!");
            //Urun entity = Repo.Query().SingleOrDefault(u => u.Id == model.Id);
            Urun entity = Repo.Query(u => u.Id == model.Id, "UrunMagazalar").SingleOrDefault();
            if (entity.UrunMagazalar != null && entity.UrunMagazalar.Count > 0)
            {
                foreach (var urunMagaza in entity.UrunMagazalar)
                {
                    _urunMagazaRepo.Delete(urunMagaza, false);
                }
                _urunMagazaRepo.Save();
            }
            entity.Adi = model.Adi.Trim();
            entity.Aciklamasi = model.Aciklamasi?.Trim();
            entity.BirimFiyati = model.BirimFiyati.Value;
            entity.KategoriId = model.KategoriId.Value;
            entity.SonKullanmaTarihi = model.SonKullanmaTarihi;
            entity.StokMiktari = model.StokMiktari.Value;
            entity.UrunMagazalar = model.MagazaIdleri.Select(magazaId => new UrunMagaza()
            {
                MagazaId = magazaId
            }).ToList();
            Repo.Update(entity);
            return new SuccessResult();
        }

        public Result<List<UrunModel>> List(UrunFilterModel filtre)
        {
            var query = Query();
            if(filtre.KategoriId.HasValue)
                query = query.Where(q => q.KategoriId == filtre.KategoriId.Value);
            if (!string.IsNullOrWhiteSpace(filtre.UrunAdi))
                query = query.Where(q => q.Adi.ToUpper().Contains(filtre.UrunAdi.ToUpper().Trim()));
            if (filtre.BirimFiyatiBaslangic != null)
                query = query.Where(q => q.BirimFiyati >= filtre.BirimFiyatiBaslangic.Value);
            if(filtre.BirimFiyatiBitis.HasValue)
                query = query.Where(q => q.BirimFiyati <= filtre.BirimFiyatiBitis.Value);
            var list = query.ToList();
            return new SuccessResult<List<UrunModel>>(list);
        }
    }
}
