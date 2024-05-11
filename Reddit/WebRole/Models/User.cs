namespace FrontReddit.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }

        public User()
        {
        }

        public User(string firstName, string lastName, string address, string city, string country, string phoneNumber, string email, string password, string image)
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