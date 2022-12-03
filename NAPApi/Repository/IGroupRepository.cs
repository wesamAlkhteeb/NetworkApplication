using Microsoft.AspNetCore.Mvc;
using NAPApi.Model;

namespace NAPApi.Repository
{
    public interface IGroupRepository 
    {
        string addGroup(String Name, int UserID);
        string deleteGroup(int GroupId, int UserId);
        List<GroupModel> FetchGroups(int UserId , int Page ,string role);
    }
}
