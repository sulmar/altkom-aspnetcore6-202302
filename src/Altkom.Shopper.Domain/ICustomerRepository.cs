namespace Altkom.Shopper.Domain;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetAll();
    Customer GetById(int id);
}
