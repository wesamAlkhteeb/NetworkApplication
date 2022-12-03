using Microsoft.AspNetCore.Mvc;
using NAPApi.Context;
using NAPApi.Entity;
using NAPApi.Model;

namespace NAPApi.Repository
{
    public class GroupRepository : IGroupRepository
    {

        private readonly ApplicationDbContext applicationDbContext;
        
        public GroupRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public string addGroup(string GroupName, int UserID)
        {
            var searchReplicate = applicationDbContext.groups.
                Where(g => g.UserId == UserID && g.GroupName == GroupName).Select(c => new {id=c.GroupId}).ToList();
            if(searchReplicate.Count() > 0)
            {
                return "fail to add. rename Group";
            }
            Group group = new Group
            {
                GroupName = GroupName,
                UserId = UserID
            };
            applicationDbContext.groups.Add(group);
            applicationDbContext.SaveChanges();


            givePermessionAdmin(UserID, group.GroupId);

            applicationDbContext.SaveChanges();
            return "Add Group Success";
        }

        private void givePermessionAdmin(int UserID  , int GroupID)
        {
            applicationDbContext.permessionsGroups.Add(new PermessionsGroup
            {
                GroupId = GroupID,
                PermessionsGroupSharedId = UserID
            });
            var AdminIDs = applicationDbContext.users.Where(u => u.RoleId == 2).Select(c => new { AdminID = c.Id }).ToList();
            foreach(var ad in AdminIDs)
            {
                applicationDbContext.permessionsGroups.Add(new PermessionsGroup
                {
                    GroupId = GroupID,
                    PermessionsGroupSharedId = ad.AdminID
                });
                applicationDbContext.SaveChanges();
            }

        }

        public string deleteGroup(int GroupId, int UserId)
        {
            var group = applicationDbContext.groups.
                FirstOrDefault(g => g.GroupId == GroupId && g.GroupName != "Global" && g.UserId == UserId);
            if (group == null)
            {
                return "Can't delete";
            }
            applicationDbContext.groups.Remove(group);
            applicationDbContext.SaveChanges();
            return "Delete Group Success";
        }

        public List<GroupModel> FetchGroups(int UserId , int page , string role)
        {
            page -= 1;
            List<GroupModel> result = new List<GroupModel>();

            var Group = applicationDbContext.permessionsGroups.
                Where(p => p.PermessionsGroupSharedId == UserId).
                Join(
                    applicationDbContext.groups,
                    permission => permission.GroupId,
                    group => group.GroupId,
                    (PermessionsGroup, group) => new
                    {
                        GroupID = group.GroupId,
                        GroupName = group.GroupName,
                    }
                    ).Skip(page * 10).Take(page * 10 + 10).ToList();
            foreach (var g in Group)
            {
                result.Add(
                    new GroupModel
                    {
                        GroupId = g.GroupID,
                        GroupName = g.GroupName,
                    });
            }

            return result;
        }
    }
}
