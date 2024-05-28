using Common.Entities;
using System.Linq;

namespace Common.Interfaces
{
    public interface IFavouritesRepository
    {
        void Create(Favourites favourites);
        IQueryable<Favourites> ReadAll();
    }
}