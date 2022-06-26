using OnlineShop.DataAccess.Models;

namespace OnlineShopAdmin.Models
{
    public class CustomerViewModel
    {
        public int PersonID { get; set; }
        public List<Address> addresses { get; set; }
        public int NameStyle { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public int EmailId { get; set; }
        public string Email { get; set; }
        public int PasswordId { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
    }
}
