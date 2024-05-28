using System.Collections.Generic;

namespace AdminTools.AdminManagement
{
    public class Administrators
    {
        public List<Admin> Admins = new List<Admin>();

        public Administrators()
        {
            Admin admin = new Admin("admin", "admin");
            Admins.Add(admin);
        }
    }
}