using Microsoft.AspNetCore.Mvc;
using OnlineShop.DataAccess.Models;
using OnlineShop.DataAccess.Repository.Abstractions;
using OnlineShopAdmin.Models;
using System.Collections.Generic;

namespace OnlineShopAdmin.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IRepository<Customer> _customerRepository;
        IRepository<Address> _addressRepository;
        IRepository<BusinessEntity> _BusinessEntityRepository;
        IRepository<BusinessEntityAddress> _BusinessEntityAddressRepository;
        IRepository<EmailAddress> _EmailAddressRepository;
        IRepository<Password> _PasswordRepository;
        IRepository<Person> _PersonRepository;

        public CustomerController(ILogger<HomeController> logger,
            IRepository<Customer> customerRepository,
            IRepository<Address> addressRepository,
            IRepository<BusinessEntity> BusinessEntityRepository,
            IRepository<BusinessEntityAddress> BusinessEntityAddressRepository,
            IRepository<EmailAddress> EmailAddressRepository,
            IRepository<Password> PasswordRepository,
            IRepository<Person> PersonRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _BusinessEntityRepository = BusinessEntityRepository;
            _BusinessEntityAddressRepository = BusinessEntityAddressRepository;
            _EmailAddressRepository = EmailAddressRepository;
            _PasswordRepository = PasswordRepository;
            _PersonRepository = PersonRepository;
        }

        public IActionResult Index()
        {
            var customers = _customerRepository.GetAll().ToList();
            customers.Reverse();
            IEnumerable<Address> addresses;
            List<CustomerViewModel> customerModels = new List<CustomerViewModel>();
            foreach (var customer in customers.Take(10))
            {
                Person person = _PersonRepository.Get(o => o.BusinessEntityId == customer.PersonId).FirstOrDefault();
                EmailAddress email = _EmailAddressRepository.Get(o => o.BusinessEntityId == customer.PersonId).FirstOrDefault();
                Password password = _PasswordRepository.Get(o => o.BusinessEntityId == customer.PersonId).FirstOrDefault();
                if(person != null && customer != null && password != null && email != null)
                {
                    int id = (int)customer.PersonId;
                    addresses = GetAddresses(id);
                    customerModels.Add(
                         new CustomerViewModel()
                         {
                             PersonID = (int)customer.PersonId,
                             addresses = addresses.ToList(),
                             Email = email.EmailAddress1,
                             EmailId = email.EmailAddressId,
                             LastName = person.LastName,
                             FirstName = person.FirstName,
                             Suffix = person.Suffix,
                             NameStyle = 0,
                             PasswordHash = password.PasswordHash,
                             PasswordId = password.BusinessEntityId
                         }
                    );
                }
                
            }
            return View(customerModels);
        }

        public IEnumerable<Address> GetAddresses(int PersonId)
        {
            var entities = _BusinessEntityAddressRepository.Get(o => o.BusinessEntityId == PersonId);
            foreach (var entity in entities)
            {
                yield return _addressRepository.Get(o => o.AddressId == entity.AddressId).FirstOrDefault();
            }
        }
    }
}
