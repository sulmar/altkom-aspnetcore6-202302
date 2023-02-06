using Microsoft.Extensions.Options;

namespace Altkom.Shopper.Api;

public interface ICurrencyService
{
    decimal GetRatio();
}

public class NbpCurrencyServiceOptions
{
    public string Url { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencyTable { get; set; }
}

public class NbpCurrencyService : ICurrencyService
{
    private readonly NbpCurrencyServiceOptions options;
    public NbpCurrencyService(IOptions<NbpCurrencyServiceOptions> options)
    {
        this.options = options.Value;
    }

    public decimal GetRatio()
    {
        throw new NotImplementedException();
    }
}
