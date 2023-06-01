namespace SalePoint.Primitives.Interfaces
{
    public interface ISaleRepository
    {
        Task<int> SellItems(List<SellerItemsType> sellerItemsTypes);

        Task<List<Sale>> GetSalesByUserId(FilterSaleProducts filterSaleProducts);

        Task<int> ReturnProduct(ProductReturns productReturns);
    }
}