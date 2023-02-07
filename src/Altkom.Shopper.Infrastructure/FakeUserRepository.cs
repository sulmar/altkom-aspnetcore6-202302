using Altkom.Shopper.Domain;

namespace Altkom.Shopper.Infrastructure;

public class FakeUserRepository : IUserRepository
{
    private readonly IDictionary<int, User> _users;

    public FakeUserRepository()
    {
        _users = new Dictionary<int, User>()
        {
            [1] = new User { Id = 1, FirstName = "John", LastName = "Smith", Email = "john@domain.com"},
            [2] = new User { Id = 2, FirstName = "Ann", LastName = "Smith", Email = "ann@domain.com" },
            [3] = new User { Id = 3, FirstName = "Kate", LastName = "Smith", Email = "kate@domain.com" },
        };
    }

    public void Add(User entity)
    {
        _users.Add(entity.Id, entity);
    }

    public bool Exists(int id)
    {
        return _users.ContainsKey(id);
    }

    public IEnumerable<User> GetAll()
    {
        return _users.Values;
    }

    public User GetByEmail(string email)
    {
        return _users.Values.SingleOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public User GetById(int id)
    {
        return _users[id];
    }

    public void Remove(int id)
    {
        _users.Remove(id);
    }

    public void Update(User entity)
    {
        throw new NotImplementedException();
    }
}
