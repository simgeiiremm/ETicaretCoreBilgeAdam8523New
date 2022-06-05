using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services
{
    public interface ISehirService : IService<SehirModel, Sehir, ETicaretContext>
    {
        Result<List<SehirModel>> List(int ulkeId);
    }

    public class SehirService : ISehirService
    {
        public RepoBase<Sehir, ETicaretContext> Repo { get; set; } = new Repo<Sehir, ETicaretContext>();

        public Result Add(SehirModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<SehirModel> Query()
        {
            return Repo.Query("Ulke").OrderBy(s => s.Adi).Select(s => new SehirModel()
            {
                Adi = s.Adi,
                Id = s.Id,
                UlkeId = s.UlkeId,

                UlkeAdiDisplay = s.Ulke.Adi
            });
        }

        public Result Update(SehirModel model)
        {
            throw new NotImplementedException();
        }

        // select * from Sehirler where ulkeId = 1
        public Result<List<SehirModel>> List(int ulkeId)
        {
            var list = Query().Where(s => s.UlkeId == ulkeId).ToList();
            return new SuccessResult<List<SehirModel>>(list);
        }
    }
}
