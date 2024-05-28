using Common.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IFavouritesService
    {
        [OperationContract]
        void AddToFavourites(Favourites favourites);
        [OperationContract]
        List<Favourites> GetAllFavourites();
    }
}