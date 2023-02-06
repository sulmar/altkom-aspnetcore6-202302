using Altkom.Shopper.Domain;

namespace Altkom.Shopper.Infrastructure;

public class DbCustomerRepository : ICustomerRepository
{
    private readonly ShopperDb db;

    public DbCustomerRepository(ShopperDb db)
    {
        this.db = db;
    }

    public void Add(Customer customer)
    {
        db.Customers.Add(customer);
        db.SaveChanges();
    }

    public IEnumerable<Customer> GetAll()
    {
        return db.Customers.ToList();
    }

    public Customer GetById(int id)
    {
        return db.Customers.Find(id);
    }
}