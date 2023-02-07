namespace Altkom.Shopper.Domain;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetAll();
    Customer GetById(int id);
    void Add(Customer customer);
    void Update(Customer customer);
    void Remove(int id);
    bool Exists(int id);
}
