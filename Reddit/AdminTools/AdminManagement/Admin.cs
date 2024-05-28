namespace AdminTools.AdminManagement
{
    public class Admin
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public Admin(string userName, string passowrd)
        {
            UserName = userName;
            Password = passowrd;
        }
    }
}