using Altkom.Shopper.Domain;

namespace Altkom.Shopper.Infrastructure;

public class DbCustomerRepository : ICustomerRepository
{
    public IEnumerable<Customer> GetAll()
    {
        throw new NotImplementedException();
    }

    public Customer GetById(int id)
    {
        throw new NotImplementedException();
    }
}