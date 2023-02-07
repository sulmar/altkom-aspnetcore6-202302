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

    public bool Exists(int id)
    {
        return db.Customers.Any(c => c.Id == id);
    }

    public IEnumerable<Customer> GetAll()
    {
        return db.Customers.ToList();
    }

    public Customer GetById(int id)
    {
        return db.Customers.Find(id);
    }

    public void Remove(int id)
    {
        var customer = new Customer { Id = id };
        db.Customers.Remove(customer);
        db.SaveChanges();
    }

    public void Update(Customer customer)
    {
        db.Customers.Update(customer);
        db.SaveChanges();
    }
}