using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketManagementSystem
{
    
    class User
    {
        public static User CurrentUser { get; set; }
        public int id { get; private set; }
        public string username { get; private set; }
        public string firstname { get; private set; }
        public string lastname { get; private set; }
        public string password { get; private set; }
        public string roll {  get; private set; }


        public User(int id)
        {
            var userInfo = DatabaseManager.GetUserInfoById(id);
            if (userInfo != null)
            {
                this.id = id;
                this.username = userInfo.ContainsKey("username") ? userInfo["username"] : string.Empty;
                this.firstname = userInfo.ContainsKey("firstname") ? userInfo["firstname"] : string.Empty;
                this.lastname = userInfo.ContainsKey("lastname") ? userInfo["lastname"] : string.Empty;
                this.password = userInfo.ContainsKey("password") ? userInfo["password"] : string.Empty;
                this.roll = userInfo.ContainsKey("roll") ? userInfo["roll"] : string.Empty;
            }
            else
            {
                throw new Exception("User not found.");
            }
        }
        public void UpdateUserInfo(string username, string firstname, string lastname)
        {
            this.username = username;
            this.firstname = firstname;
            this.lastname = lastname;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname))
            {
                throw new Exception("All fields are required.");
            }
            else if (DatabaseManager.IsUsernameAveilable(username))
            {
                DatabaseManager.UpdateUserInfo(this.id, username, firstname, lastname);
            }
                
        }
        public static void ChangeUserPassword(string username, string currentPassword, string newPassword)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                throw new Exception("All fields are required.");
            }
            else if (DatabaseManager.ValidateUser(username, currentPassword))
            {
                DatabaseManager.ChangeUserPassword(username, newPassword);
               
            }

        }

    }

}
