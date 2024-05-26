using Common.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IPostService
    {
        [OperationContract]
        void AddPost(PostData post);
        [OperationContract]
        List<PostData> GetAllPosts();
        [OperationContract]
        PostData GetPost(int id);
        [OperationContract]
        bool RemovePost(int id);
    }
}