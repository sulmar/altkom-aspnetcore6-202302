namespace Altkom.Shopper.Domain;

public interface ICustomerRepository : IEntityRepository<Customer>
{
   
}

public interface IUserRepository : IEntityRepository<User>
{
    User GetByEmail(string email);
}

// interfejs generyczny (ugólniony, szablon)
public interface IEntityRepository<TEntity>
    where TEntity : BaseEntity
{
    IEnumerable<TEntity> GetAll();
    TEntity GetById(int id);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(int id);
    bool Exists(int id);
}
