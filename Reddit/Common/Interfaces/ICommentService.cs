using Common.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface ICommentService
    {
        [OperationContract]
        void AddComment(CommentData comment);
        [OperationContract]
        List<CommentData> GetAllComments();
        [OperationContract]
        CommentData GetComment(int id);
    }
}