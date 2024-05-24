using Microsoft.WindowsAzure.Storage.Table;

namespace Common.Entities
{
    public class UserData : TableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }

        public UserData(string email)
        {
            PartitionKey = "User";
            RowKey = email;
        }

        public UserData() { }

        public UserData(string firstName, string lastName, string address, string city, string country, string phoneNumber, string email, string password, string image) : this(firstName)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            City = city;
            Country = country;
            PhoneNumber = phoneNumber;
            Email = email;
            Password = password;
            Image = image;
        }
    }
}