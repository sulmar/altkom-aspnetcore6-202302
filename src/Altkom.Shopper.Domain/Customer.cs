namespace Altkom.Shopper.Domain;

public class Base
{
    
}

public class BaseEntity : Base
{
    public int Id { get; set; }
}

public class Customer : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}