using System;
using System.Collections.Generic;
using WiseCartLogic.Entities;

namespace WiseCartLogic
{
    [Serializable]
    public class UsersDB
    {
        public List<User> Users { get; set; }

        public UsersDB()
        {
            Users = new List<User>();
        }
    }
}
