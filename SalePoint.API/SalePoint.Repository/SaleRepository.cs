using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using static Dapper.SqlMapper;

namespace SalePoint.Repository
{
    public class SaleRepository(IConfiguration configuration) : ISaleRepository
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<List<Sale>> GetSalesByUserId(FilterSaleProducts filterSaleProducts)
        {
            try
            {
                List<Sale> sales = [];
                DynamicParameters parameters = new();
                parameters.Add("userId", filterSaleProducts.UserId);
                parameters.Add("saleDateStart", filterSaleProducts.SaleDateStart);
                parameters.Add("saleDateEnd", filterSaleProducts.SaleDateEnd);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                sales = (await conn.QueryAsync<Sale>("ShowSalesByUserId", param: parameters, commandType: CommandType.StoredProcedure)).ToList();
                conn.Close();

                return sales;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> ReturnProduct(ProductReturns productReturns)
        {
            try
            {
                int idProductReturn = 0;
                DynamicParameters parameters = new();
                parameters.Add("SaleId", productReturns.SaleId);
                parameters.Add("BoxCutId", productReturns.BoxCutId);
                parameters.Add("ProductId", productReturns.ProductId);
                parameters.Add("UserId", productReturns.UserId);
                parameters.Add("Quantity", productReturns.Quantity);
                //parameters.Add("PurchasePrice", productReturns.PurchasePrice);
                //parameters.Add("RetailPrice", productReturns.RetailPrice);
                parameters.Add("RetailGain", productReturns.RetailGain);
                //parameters.Add("WholesalePrice", productReturns.WholesalePrice);
                parameters.Add("WholesaleGain", productReturns.WholesaleGain);
                //parameters.Add("Wholesale", productReturns.Wholesale);
                parameters.Add("Revenue", productReturns.Revenue);
                //parameters.Add("UnitMeasureId", productReturns.UnitMeasureId);
                //parameters.Add("ReturnDate", productReturns.ReturnDate);

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                idProductReturn = await conn.QueryFirstAsync<int>("ReturnProduct", param: parameters, commandType: CommandType.StoredProcedure);
                conn.Close();

                return idProductReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> SellItems(List<SellerItemsType> sellerItemsTypes)
        {
            try
            {
                // create DataTable
                DataTable sellerItemsDT = new();
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.BoxCutId), typeof(int));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.ProductId), typeof(int));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.UserId), typeof(int));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.Quantity), typeof(decimal));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.PurchasePrice), typeof(decimal));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.RetailPrice), typeof(decimal));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.RetailGain), typeof(decimal));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.WholesalePrice), typeof(decimal));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.WholesaleGain), typeof(decimal));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.Wholesale), typeof(decimal));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.Amount), typeof(decimal));
                sellerItemsDT.Columns.Add(nameof(SellerItemsType.UnitMeasureId), typeof(int));

                // add rows to DataTable
                foreach (SellerItemsType sellerItemsType in sellerItemsTypes)
                {
                    DataRow row = sellerItemsDT.NewRow();

                    row[nameof(SellerItemsType.BoxCutId)] = sellerItemsType.BoxCutId ?? (object)DBNull.Value;
                    row[nameof(SellerItemsType.UserId)] = sellerItemsType.UserId;
                    row[nameof(SellerItemsType.ProductId)] = sellerItemsType.ProductId;
                    row[nameof(SellerItemsType.Quantity)] = sellerItemsType.Quantity;
                    row[nameof(SellerItemsType.PurchasePrice)] = sellerItemsType.PurchasePrice;
                    row[nameof(SellerItemsType.RetailPrice)] = sellerItemsType.RetailPrice;
                    row[nameof(SellerItemsType.RetailGain)] = sellerItemsType.RetailGain;
                    row[nameof(SellerItemsType.WholesalePrice)] = sellerItemsType.WholesalePrice;
                    row[nameof(SellerItemsType.WholesaleGain)] = sellerItemsType.WholesaleGain;
                    row[nameof(SellerItemsType.Wholesale)] = sellerItemsType.Wholesale;
                    row[nameof(SellerItemsType.Amount)] = sellerItemsType.Amount;
                    row[nameof(SellerItemsType.UnitMeasureId)] = sellerItemsType.UnitMeasureId;
                    sellerItemsDT.Rows.Add(row);
                }

                // create parameters
                var parameters = new
                {
                    sellerItemsType = sellerItemsDT.AsTableValuedParameter("[dbo].[SellerItemsType]")
                };

                using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                // execute Stored Procedure
                return await conn.ExecuteScalarAsync<int>(
                    "[dbo].[SellItems]",
                    param: parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}