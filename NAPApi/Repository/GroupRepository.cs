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

        public string addGroup(string GroupName, int UserID,string role)
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


            givePermessionAdmin(UserID, group.GroupId,role);
            applicationDbContext.SaveChanges();
            
            return "Add Group Success";
        }

        private void givePermessionAdmin(int UserID  , int GroupID,string role)
        {
            applicationDbContext.permessionsGroups.Add(new PermessionsGroup
            {
                GroupId = GroupID,
                PermessionsGroupSharedId = UserID
            });
            applicationDbContext.SaveChanges();
            if (role == "ADMIN")
            {
                return;
            }
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
            var isReversation = (from fl in applicationDbContext.files
                                 join gr in applicationDbContext.groups
                                    on fl.GroupId equals gr.GroupId
                                 where fl.FileIdUses != -1
                                 select new
                                 {
                                     GroupId = gr.GroupId
                                 });
            if(isReversation.Count() >0)
            {
                return "can't delete there are file is reservation";
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

        public string GrantPermission(int IdUser, int IdGroup, string UserName)
        {
            var user = (from gr in applicationDbContext.groups 
                        where gr.GroupId == IdGroup && gr.UserId == IdUser 
                        select gr.GroupId).ToList();
            if( user.Count() == 0)
            {
                return "You are not the owner of this group";
            }
            var idUserFriend = (from ur in applicationDbContext.users
                                where ur.Username == UserName
                                select ur.RoleId).ToList();
            if (idUserFriend.Count() == 0)
            {
                return "the account your friend is not registered";
            }

            var isHave = (from prg in applicationDbContext.permessionsGroups
                                where prg.PermessionsGroupSharedId == idUserFriend[0] && prg.GroupId == IdGroup
                          select prg.PermessionsGroupId).ToList();
            if (isHave.Count() > 0)
            {
                return "your friend has permession" + idUserFriend[0];
            }
            applicationDbContext.permessionsGroups.Add(new PermessionsGroup
            {
                GroupId= IdGroup,
                PermessionsGroupSharedId = idUserFriend[0]
            });
            applicationDbContext.SaveChanges();
            return "Permession added";
        }

        public string NoGrantPermission(int IdUser, int IdGroup, int IdUserFriend)
        {
            var isHas = (from gr in applicationDbContext.groups
                         where gr.UserId == IdUser && gr.GroupId == IdGroup
                         select gr.GroupId
                         ).ToList();
            if(isHas.Count() == 0)
            {
                return "You are not the owner of this group";
            }
            var isFriendAdd = (from prg in applicationDbContext.permessionsGroups
                               where prg.PermessionsGroupSharedId == IdUserFriend && prg.GroupId == IdGroup
                               select prg.PermessionsGroupId
                               ).ToList();
            if (isFriendAdd.Count() == 0)
            {
                return "this name has not any permission";
            }
            var isAdmin = (from us in applicationDbContext.users
                           where us.Id == IdUserFriend && us.RoleId==2                           
                           select us.RoleId ).ToList();
            if(isAdmin.Count() >0)
            {
                return "Can't romve admin";
            }
            var prGroup = (from prg in applicationDbContext.permessionsGroups 
                                        where prg.PermessionsGroupId == isFriendAdd.First() 
                                        select prg).SingleOrDefault();
            if(prGroup == null)
            {
                return "this name has not any permission";
            }
            applicationDbContext.permessionsGroups.Remove(prGroup);
            applicationDbContext.SaveChanges();
            return "Permession removed"; 
        }

        public List<UserPermessionGroup> FetchUserPermession(int IdUser, int IdGroup, int Page)
        {
            List<UserPermessionGroup> groups = new List<UserPermessionGroup>();

            var users = (from gr in applicationDbContext.groups
                         join grp in applicationDbContext.permessionsGroups
                             on gr.GroupId equals grp.GroupId
                         join us in applicationDbContext.users
                            on grp.PermessionsGroupSharedId equals us.Id
                         where grp.GroupId == IdGroup && gr.UserId == IdUser
                         select new
                         {
                             Id = us.Id,
                             username = us.Username
                         }).Distinct().Skip(((Page-1) * 10)).Take(((Page-1) * 10)+10).ToList();

            foreach(var user in users)
            {
                groups.Add(new UserPermessionGroup
                {
                    Id = user.Id,
                    Name = user.username
                });
            }
            return groups;
        }
    }
}