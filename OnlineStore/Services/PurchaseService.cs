using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Services
{
    public interface IPurchaseService
    {
        IEnumerable<Purchase> GetPurchaseRecord();
    }
}
