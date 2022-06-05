using DataAccess.Contexts;
using DataAccess.Repositories.Bases;

namespace DataAccess.Repositories
{
    public class KategoriRepo : KategoriRepoBase
    {
        public KategoriRepo() : base()
        {

        }

        public KategoriRepo(ETicaretContext eTicaretContext) : base(eTicaretContext)
        {

        }
    }
}
