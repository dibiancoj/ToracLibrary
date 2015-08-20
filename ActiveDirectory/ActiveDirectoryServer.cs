using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.ActiveDirectory
{

    /// <summary>
    /// Class Used to Interact With Your Active Directory System
    /// </summary>
    /// <remarks>Class is immutable</remarks>
    public class ActiveDirectory : IDisposable
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DomainName">Domain Name</param>
        /// <example>Navigators.internal</example>
        public ActiveDirectory(string DomainName)
        {
            //validate the parameters passed in
            if (string.IsNullOrEmpty(DomainName))
            {
                throw new ArgumentNullException("Domain Name Can't Be Null");
            }

            //create the active directory processor instance
            ActiveDirectoryProcessor = new PrincipalContext(ContextType.Domain, DomainName);
        }

        #endregion

        #region Properties

        #region Main Connection Property To Server

        /// <summary>
        /// Main Connection To Active Directory Server
        /// </summary>
        /// <remarks>Property Is Immutable</remarks>
        public PrincipalContext ActiveDirectoryProcessor { get; }

        #endregion

        #region Dispose Properties

        /// <summary>
        /// Holds a flag if the class has been disposed yet or called to be disposed yet
        /// </summary>
        /// <remarks>Used IDisposable</remarks>
        private bool disposed { get; set; }

        #endregion

        #endregion

        #region Main Public Calls

        #region Users

        /// <summary>
        /// Check to see if a user exists in the active directory
        /// </summary>
        /// <param name="UserName">User Name To Check</param>
        /// <returns>Boolean - True if they exist</returns>
        public bool UserExists(string UserName)
        {
            //see if the user is not null and return if they are not null
            using (var UserToRetrieve = GetUser(UserName))
            {
                //return if the user is found
                return UserToRetrieve != null;
            }
        }

        /// <summary>
        /// Sets the user password
        /// </summary>
        /// <param name="UserName">The username to set</param>
        /// <param name="NewPassword">The new password to use</param>
        ///<returns>Boolean if it was successful</returns>
        public bool SetUserPassword(string UserName, string NewPassword)
        {
            //Set the users password
            using (var UserToRetrieve = GetUser(UserName))
            {
                //set the password on the user
                UserToRetrieve.SetPassword(NewPassword);
            }

            //return the boolean if successful
            return true;
        }

        /// <summary>
        /// Validates the username and password of a given user
        /// </summary>
        /// <param name="UserName">The username to validate</param>
        /// <param name="Password">The password of the username to validate</param>
        /// <returns>Returns True of user is valid</returns>
        public bool ValidateCredentials(string UserName, string Password)
        {
            //validate the credentials
            return ActiveDirectoryProcessor.ValidateCredentials(UserName, Password);
        }

        /// <summary>
        /// Enable Or Disable User
        /// </summary>
        /// <param name="UserName">UserName To Enable</param>
        /// <param name="EnableUser">Enable User...False Disables them</param>
        /// <returns>Boolean if it was successful</returns>
        public bool EnableOrDisableUser(string UserName, bool EnableUser)
        {
            //get the user first
            using (var ActiveDirectoryUser = GetUser(UserName))
            {
                //set the enable flag
                ActiveDirectoryUser.Enabled = EnableUser;

                //save the user
                ActiveDirectoryUser.Save();

                //return true and let the developer it completed successfully
                return true;
            }
        }

        /// <summary>
        /// Force A User's Password To Expire Now
        /// </summary>
        /// <param name="UserName">User Name</param>
        /// <returns>Boolean if it was successful</returns>
        public bool ForceUserPasswordToExpireNow(string UserName)
        {
            //User to enable or disable
            using (var ActiveDirectoryUser = GetUser(UserName))
            {
                //Expire the password now
                ActiveDirectoryUser.ExpirePasswordNow();

                //save the user
                ActiveDirectoryUser.Save();

                //return true and let the developer it completed successfully
                return true;
            }
        }

        /// <summary>
        /// Gets a list of the groups that the user is a part of
        /// </summary>
        /// <param name="UserName">The user you want to get the group memberships</param>
        /// <returns>Returns an Ienumerable of strings (group name). Data is lazy loaded, call To.Array() to grab the data</returns>
        public IEnumerable<string> GroupsThatUserIsInLazy(string UserName)
        {
            //let's grab the user first
            using (var ActiveDirectoryUser = GetUser(UserName))
            {
                //let's grab this person's groups now
                using (var UserGroups = ActiveDirectoryUser.GetGroups())
                {
                    //let's loop through the groups and return the names of them
                    foreach (var thisUserGroup in UserGroups)
                    {
                        //return the name now
                        yield return thisUserGroup.Name;
                    }
                }
            }
        }

        /// <summary>
        /// Is the user locked out
        /// </summary>
        /// <param name="UserName">User Name To Check</param>
        /// <returns>Boolean - true if they are locked out</returns>
        public bool IsUserLockedOut(string UserName)
        {
            //let's grab the user first
            using (var ActiveDirectoryUser = GetUser(UserName))
            {
                //return the flag
                return ActiveDirectoryUser.IsAccountLockedOut();
            }
        }

        /// <summary>
        /// Unlock a user who is locked out
        /// </summary>
        /// <param name="UserName">User Name To Unlock</param>
        /// <returns>Boolean if successful</returns>
        public bool UnlockUserWhoIsLockedOut(string UserName)
        {
            //let's grab the user first
            using (var ActiveDirectoryUser = GetUser(UserName))
            {
                //unlock the user now
                ActiveDirectoryUser.UnlockAccount();

                //Let the developer know it was successful
                return true;
            }
        }

        /// <summary>
        /// Checks if user is a member of a given group
        /// </summary>
        /// <param name="UserNameToCheckFor">The user you want to validate</param>
        /// <param name="GroupNameToCheckIn">The group you want to check the membership of the user</param>
        /// <returns>Returns true if user is a group member</returns>
        public bool IsUserGroupMember(string UserNameToCheckFor, string GroupNameToCheckIn)
        {
            //grab the user
            using (var User = GetUser(UserNameToCheckFor))
            {
                //grab the user's group
                using (var Group = GetGroup(GroupNameToCheckIn))
                {
                    //check to see if one or both are null
                    if (User == null || Group == null)
                    {
                        //don't have both items...
                        return false;
                    }
                    else
                    {
                        //we have both items, ...resurse the group and check each guy
                        using (var GroupMembers = Group.GetMembers(true))
                        {
                            //let's loop through the members now
                            foreach (var PrincipalInGroup in GroupMembers)
                            {
                                //is the principal name == the name 
                                if (string.Equals(PrincipalInGroup.Name, User.Name, StringComparison.OrdinalIgnoreCase))
                                {
                                    return true;
                                }
                            }

                        }

                        //if we get here we can't find the member, so return false
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the Principle member name gives a name
        /// </summary>
        /// <param name="UserName">The user you want to get</param>
        /// <returns>Returns member name for the user</returns>
        public string GetUserMemberName(string UserName)
        {
            //let's grab the user first
            using (var ActiveDirectoryUser = GetUser(UserName))
            {
                //return the name straight
                return ActiveDirectoryUser.Name;
            }
        }

        #endregion

        #region Groups

        /// <summary>
        /// Check to see if a group exists in the active directory
        /// </summary>
        /// <param name="GroupName">Group Name To Check</param>
        /// <returns>Boolean - True if they exist</returns>
        public bool GroupExists(string GroupName)
        {
            //see if the group is not null and return if they are not null
            using (var GroupToRetrieve = GetGroup(GroupName))
            {
                //is this user found?
                return GroupToRetrieve != null;
            }
        }

        /// <summary>
        /// Return the members of a group
        /// </summary>
        /// <param name="GroupName">Group Name</param>
        /// <returns>Ienumerable of string. Data is lazy loaded, call To.Array() to grab the data</returns>
        public IEnumerable<string> GroupMembershipListingLazy(string GroupName)
        {
            //let's grab the group, this way we can dispose of ut
            using (var Group = GetGroup(GroupName))
            {
                //Loop through the users in the groups
                foreach (var Results in Group.Members)
                {
                    //return the group names
                    yield return Results.Name;
                }
            }
        }

        #endregion

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets a certain user on Active Directory. Returns the UserPrincipal
        /// </summary>
        /// <param name="UserNameToRetrieve">The username to get</param>
        /// <returns>Returns the UserPrincipal Object</returns>
        /// <remarks>Please try to clean up and call dispose after you do whatever you need to with this user</remarks>
        private UserPrincipal GetUser(string UserNameToRetrieve)
        {
            //grab the user principal. The User Principal has a dispose method...please use it!!!
            return UserPrincipal.FindByIdentity(ActiveDirectoryProcessor, UserNameToRetrieve);
        }

        /// <summary>
        /// Gets a certain group on Active Directory. Returns the GroupPrincipal
        /// </summary>
        /// <param name="GroupNameToCheckFor">The group to get</param>
        /// <returns>Returns the GroupPrincipal Object</returns>
        /// <remarks>Please try to clean up and call dispose after you do whatever you need to with this group</remarks>
        public GroupPrincipal GetGroup(string GroupNameToCheckFor)
        {
            //return the active directory group. The Group Principal has a dispose method...please use it!!!
            return GroupPrincipal.FindByIdentity(ActiveDirectoryProcessor, GroupNameToCheckFor);
        }

        #endregion

        #region Dispose Method

        /// <summary>
        /// Disposes My Object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose Overload. Ensures my database connection is closed
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //dispose of the active directory processor
                    ActiveDirectoryProcessor.Dispose();
                }
            }
            disposed = true;
        }

        #endregion

    }

}
