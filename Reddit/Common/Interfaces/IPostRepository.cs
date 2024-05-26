using Common.Entities;
using System.Linq;

namespace Common.Interfaces
{
    public interface IPostRepository
    {
        void Create(PostData post);
        PostData Read(int id);
        IQueryable<PostData> ReadAll();
        bool Delete(int id);
    }
}