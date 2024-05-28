using Common.Entities;
using Common.Interfaces;
using Common.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace RedditService
{
    public class FavouritesServiceProvider : IFavouritesService
    {
        private readonly static FavouritesRepository repository = new FavouritesRepository();

        public void AddToFavourites(Favourites favourites)
        {
            repository.Create(new Favourites(favourites.PostId) { Id = favourites.Id, PostId = favourites.PostId, UserEmail = favourites.UserEmail });
        }

        public List<Favourites> GetAllFavourites()
        {
            return repository
                  .ReadAll()
                  .Select(fav => new Favourites
                  {
                      Id = fav.Id,
                      PostId = fav.PostId,
                      UserEmail = fav.UserEmail,
                  }).ToList();
        }
    }
}