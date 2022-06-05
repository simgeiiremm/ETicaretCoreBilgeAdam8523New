using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{

    public interface IMagazaService : IService<MagazaModel, Magaza, ETicaretContext>
    {

    }

    public class MagazaService : IMagazaService
    {
        public RepoBase<Magaza, ETicaretContext> Repo { get; set; } = new Repo<Magaza, ETicaretContext>();
        RepoBase<UrunMagaza, ETicaretContext> _urunMagazaRepo = new Repo<UrunMagaza, ETicaretContext>();

        public Result Add(MagazaModel model)
        {
            if (Repo.Query().Any(m => m.Adi.ToLower() == model.Adi.ToLower().Trim()))
                return new ErrorResult("Girilen mağaza adına sahip kayıt bulunmaktadır!");
            Magaza entity = new Magaza()
            {
                Adi = model.Adi.Trim(),
                Puani = model.Puani,
                SanalMi = model.SanalMi
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            var magaza = Repo.Query("UrunMagazalar").SingleOrDefault(m => m.Id == id);
            if (magaza.UrunMagazalar != null && magaza.UrunMagazalar.Count > 0)
            {
                foreach (var urunMagaza in magaza.UrunMagazalar)
                {
                    _urunMagazaRepo.Delete(urunMagaza, false);
                }
                _urunMagazaRepo.Save();
            }
            //Repo.Delete(magaza);
            Repo.Delete(m => m.Id == id);
            return new SuccessResult();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<MagazaModel> Query()
        {
            return Repo.Query().OrderByDescending(m => m.SanalMi).ThenBy(m => m.Adi).Select(m => new MagazaModel()
            {
                Id = m.Id,
                Adi = m.Adi,
                Puani = m.Puani,
                SanalMi = m.SanalMi,
                SanalMiDisplay = m.SanalMi ? "Evet" : "Hayır"
            });
        }

        public Result Update(MagazaModel model)
        {
            if (Repo.Query().Any(m => m.Adi.ToLower() == model.Adi.ToLower().Trim() && m.Id != model.Id))
                return new ErrorResult("Girilen mağaza adına sahip kayıt bulunmaktadır!");
            Magaza entity = Repo.Query(m => m.Id == model.Id).SingleOrDefault();
            entity.Adi = model.Adi.Trim();
            entity.Puani = model.Puani;
            entity.SanalMi = model.SanalMi;
            Repo.Update(entity);
            return new SuccessResult();
        }
    }
}
