using Common.Entities;
using System.Linq;

namespace Common.Interfaces
{
    public interface ICommentRepository
    {
        void Create(CommentData comment);
        CommentData Read(int id);
        IQueryable<CommentData> ReadAll();
    }
}