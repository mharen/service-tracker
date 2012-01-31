using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using service_tracker_mvc.Data;
using service_tracker_mvc.Models;

namespace service_tracker_mvc
{
    public class SimpleRoleProvider : RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }


        private string _ApplicationName;
        public override string ApplicationName
        {
            get
            {
                return _ApplicationName;
            }
            set
            {
                _ApplicationName = value;
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return Enum.GetNames(typeof(RoleType)).ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var db = new DataContext())
            {
                var RoleId = db.Users.Single(u => u.ClaimedIdentifier == username).RoleId;

                // return all roles up to and including the requested role
                return Enum.GetNames(typeof(RoleType)).Take(RoleId).ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (var db = new DataContext())
            {
                int DesiredRoleId = (int)Enum.Parse(typeof(RoleType), roleName);
                int UserRoleId = db.Users.Single(u => u.ClaimedIdentifier == username).RoleId;

                return DesiredRoleId >= UserRoleId;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}