using Microsoft.AspNetCore.Mvc;
using NAPApi.Model;

namespace NAPApi.Repository
{
    public interface IGroupRepository 
    {
        string addGroup(String Name, int UserID, string role);
        string deleteGroup(int GroupId, int UserId);
        List<GroupModel> FetchGroups(int UserId , int Page ,string role);
        string GrantPermission(int idUser , int idGroup , string userName);
        string NoGrantPermission(int idUser, int idGroup , int IdUserFriend);
        List<UserPermessionGroup> FetchUserPermession(int idUser, int idGroup ,int page);
    }
}
