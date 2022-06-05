using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework;
using AppCore.DataAccess.EntityFramework.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services
{
    public interface IUlkeService :IService<UlkeModel, Ulke, ETicaretContext>
    {

    }

    public class UlkeService : IUlkeService
    {
        public RepoBase<Ulke, ETicaretContext> Repo { get; set; } = new Repo<Ulke, ETicaretContext>();

        public Result Add(UlkeModel model)
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

        public IQueryable<UlkeModel> Query()
        {
            return Repo.Query().OrderBy(u => u.Adi).Select(u => new UlkeModel()
            {
                Adi = u.Adi,
                Id = u.Id
            });
        }

        public Result Update(UlkeModel model)
        {
            throw new NotImplementedException();
        }
    }
}
