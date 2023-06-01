using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System;
using System.Data;

namespace SalePoint.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateProduct(Product product)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("Name", product.Name);
                dynamicParameters.Add("BarCode", product.BarCode);
                dynamicParameters.Add("ExpirationDate", product.ExpirationDate);
                dynamicParameters.Add("Description", product.Description);
                dynamicParameters.Add("Stock", product.Stock);
                dynamicParameters.Add("MinimumStock", product.MinimumStock);
                dynamicParameters.Add("PurchasePrice", product.PurchasePrice);
                dynamicParameters.Add("Thumbnail", product.Thumbnail, DbType.Binary);
                dynamicParameters.Add("UnitMeasureId", product.UnitMeasureId);
                dynamicParameters.Add("UserId", product.UserId);
                dynamicParameters.Add("DepartmentId", product.Department!.Id);

                // create DataTable
                DataTable priceProductDT = new();
                priceProductDT.Columns.Add(nameof(PriceProduct.ProductId), typeof(int));
                priceProductDT.Columns.Add(nameof(PriceProduct.SalesPrice), typeof(decimal));
                priceProductDT.Columns.Add(nameof(PriceProduct.PercentageProfit), typeof(decimal));
                priceProductDT.Columns.Add(nameof(PriceProduct.Revenue), typeof(decimal));
                priceProductDT.Columns.Add(nameof(PriceProduct.Wholesale), typeof(int));

                // add rows to DataTable
                foreach (PriceProduct priceProductType in product.PriceProducts!)
                {
                    DataRow row = priceProductDT.NewRow();

                    row[nameof(PriceProduct.ProductId)] = priceProductType.ProductId;
                    row[nameof(PriceProduct.SalesPrice)] = priceProductType.SalesPrice;
                    row[nameof(PriceProduct.PercentageProfit)] = priceProductType.PercentageProfit;
                    row[nameof(PriceProduct.Revenue)] = priceProductType.Revenue;
                    row[nameof(PriceProduct.Wholesale)] = priceProductType.Wholesale;
                    priceProductDT.Rows.Add(row);
                }

                dynamicParameters.Add("priceProductType", priceProductDT.AsTableValuedParameter("[dbo].[PriceProductType]"));

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                int productId = await conn.QuerySingleOrDefaultAsync<int>("CreateProduct", dynamicParameters, commandType: CommandType.StoredProcedure);
                conn.Dispose();

                return productId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts()
        {
            try
            {
                IEnumerable<ProductModel> products;

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                products = await conn.QueryAsync<ProductModel>("GetAllProducts", commandType: CommandType.StoredProcedure);
                conn.Dispose();

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsExpiringSoon()
        {
            try
            {
                IEnumerable<Product> products;

                string query = @"
                                   SELECT 
		                                   P.[Id] 'Id',	
		                                   P.[Name] 'Name',
		                                   P.[BarCode] 'Barcode',
		                                   P.[ExpirationDate] 'ExpirationDate',
		                                   P.[Description] 'Description',
		                                   P.[Stock] 'Stock',
		                                   P.[MinimumStock] 'MinimumStock',
		                                   P.[PurchasePrice] 'PurchasePrice',
		                                   P.[Thumbnail] 'Thumbnail',
		                                   P.[UnitMeasureId] 'UnitMeasureId',
		                                   P.[IsActive] 'IsActive',
		                                   P.[CreationDate] 'CreationDate',
		                                   P.[ModificationDate] 'ModificationDate',
		                                   P.[DeletionDate] 'DeletionDate',
		                                   P.[UserId] 'UserId',		                                   
		                                   PD.[Id] 'Id',
		                                   PD.[ProductId] 'ProductId',
		                                   PD.[DepartmentId] 'DepartmentId',
		                                   D.[Id] 'Id',
		                                   D.[Name] 'Name',
		                                   D.[IsActive] 'IsActive',
		                                   MU.[Id] 'Id',
		                                   MU.[Name] 'Name',
		                                   MU.[Icon] 'Icon'
		                                FROM ProductDepartment PD
		                                INNER JOIN Product P ON PD.ProductId = P.Id
		                                INNER JOIN Department D ON PD.DepartmentId = D.Id
		                                INNER JOIN MeasurementUnit MU ON P.UnitMeasureId = MU.Id
		                                WHERE P.IsActive = 1
		                                AND (DATEDIFF(DAY, GETDATE(), P.ExpirationDate) < 100  AND DATEDIFF(DAY, GETDATE(), P.ExpirationDate)  >= 0)";

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                products = await conn.QueryAsync<Product, ProductDepartment, Department, MeasurementUnit, Product>(query,
                    (product, productDepartment, department, measurementUnit) =>
                    {
                        product.Department = department;
                        product.ProductDepartment = productDepartment;
                        product.MeasurementUnit = measurementUnit;
                        return product;
                    }, splitOn: "Id,Id,Id");

                conn.Dispose();

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsNearCompletition()
        {
            try
            {
                IEnumerable<Product> products;

                string query = @"
                                   SELECT 
		                                   P.[Id] 'Id',	
		                                   P.[Name] 'Name',
		                                   P.[BarCode] 'Barcode',
		                                   P.[ExpirationDate] 'ExpirationDate',
		                                   P.[Description] 'Description',
		                                   P.[Stock] 'Stock',
		                                   P.[MinimumStock] 'MinimumStock',
		                                   P.[PurchasePrice] 'PurchasePrice',
		                                   P.[Thumbnail] 'Thumbnail',
		                                   P.[UnitMeasureId] 'UnitMeasureId',
		                                   P.[IsActive] 'IsActive',
		                                   P.[CreationDate] 'CreationDate',
		                                   P.[ModificationDate] 'ModificationDate',
		                                   P.[DeletionDate] 'DeletionDate',
		                                   P.[UserId] 'UserId',		                                   
		                                   PP.[Wholesale] 'Wholesale',
		                                   PD.[Id] 'Id',
		                                   PD.[ProductId] 'ProductId',
		                                   PD.[DepartmentId] 'DepartmentId',
		                                   D.[Id] 'Id',
		                                   D.[Name] 'Name',
		                                   D.[IsActive] 'IsActive',
		                                   MU.[Id] 'Id',
		                                   MU.[Name] 'Name',
		                                   MU.[Icon] 'Icon'
		                                FROM ProductDepartment PD
		                                INNER JOIN Product P ON PD.ProductId = P.Id
		                                INNER JOIN PriceProduct PP ON P.Id = PP.ProductId
		                                INNER JOIN Department D ON PD.DepartmentId = D.Id
		                                INNER JOIN MeasurementUnit MU ON P.UnitMeasureId = MU.Id
		                                WHERE P.IsActive = 1
	                                    AND Stock <= MinimumStock;";

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                products = await conn.QueryAsync<Product, ProductDepartment, Department, MeasurementUnit, Product>(query,
                    (product, productDepartment, department, measurementUnit) =>
                    {
                        product.Department = department;
                        product.ProductDepartment = productDepartment;
                        product.MeasurementUnit = measurementUnit;
                        return product;
                    }, splitOn: "Id,Id,Id");

                conn.Dispose();

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                Product product;

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                IEnumerable<Product> products = await conn.QueryAsync<Product, PriceProduct, ProductDepartment, Department, MeasurementUnit, Product>("GetProductById",
                     (product, priceProduct, productDepartment, department, measurementUnit) =>
                     {
                         product.Department = department;
                         product.ProductDepartment = productDepartment;
                         product.MeasurementUnit = measurementUnit;
                         product.PriceProducts!.Add(priceProduct);
                         return product;
                     },
                     param: new { productId },
                     splitOn: "Id,Id,Id",
                     commandType: CommandType.StoredProcedure);

                product = products.GroupBy(p => p.Id).Select(g =>
                {
                    var groupedPost = g.First();
                    groupedPost.PriceProducts = g.Select(p => p.PriceProducts!.Single()).ToList();
                    return groupedPost;
                }).First();

                conn.Dispose();

                return product;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductByBarCode(string barCode)
        {
            try
            {
                IEnumerable<Product> products;

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                products = await conn.QueryAsync<Product, PriceProduct, ProductDepartment, Department, MeasurementUnit, Product>("GetProductByBarCode",
                    (product, priceProduct, productDepartment, department, measurementUnit) =>
                    {
                        product.Department = department;
                        product.ProductDepartment = productDepartment;
                        product.MeasurementUnit = measurementUnit;
                        product.PriceProducts!.Add(priceProduct);
                        return product;
                    },
                    param: new { barCode },
                    splitOn: "Id,Id,Id",
                    commandType: CommandType.StoredProcedure);

                products = products.GroupBy(p => p.Id).Select(g =>
                {
                    var groupedPost = g.First();
                    groupedPost.PriceProducts = g.Select(p => p.PriceProducts!.Single()).ToList();
                    return groupedPost;
                });

                conn.Dispose();

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProductModel>> GetProductByNameOrDescription(string keyWord)
        {
            try
            {
                IEnumerable<ProductModel> products;

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();

                products = await conn.QueryAsync<ProductModel>("GetProductByNameOrDescription", param: new { keyWord }, commandType: CommandType.StoredProcedure);

                conn.Dispose();

                return products;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateProduct(Product product)
        {
            try
            {
                DynamicParameters dynamicParameters = new();
                dynamicParameters.Add("Id", product.Id);
                dynamicParameters.Add("Name", product.Name);
                dynamicParameters.Add("BarCode", product.BarCode);
                dynamicParameters.Add("ExpirationDate", product.ExpirationDate);
                dynamicParameters.Add("Description", product.Description);
                dynamicParameters.Add("Stock", product.Stock);
                dynamicParameters.Add("MinimumStock", product.MinimumStock);
                dynamicParameters.Add("PurchasePrice", product.PurchasePrice);
                dynamicParameters.Add("Thumbnail", product.Thumbnail, DbType.Binary);
                dynamicParameters.Add("UnitMeasureId", product.UnitMeasureId);
                dynamicParameters.Add("UserId", product.UserId);
                dynamicParameters.Add("DepartmentId", product.Department!.Id);

                // create DataTable
                DataTable priceProductDT = new();
                priceProductDT.Columns.Add(nameof(PriceProduct.ProductId), typeof(int));
                priceProductDT.Columns.Add(nameof(PriceProduct.SalesPrice), typeof(decimal));
                priceProductDT.Columns.Add(nameof(PriceProduct.PercentageProfit), typeof(decimal));
                priceProductDT.Columns.Add(nameof(PriceProduct.Revenue), typeof(decimal));
                priceProductDT.Columns.Add(nameof(PriceProduct.Wholesale), typeof(int));

                // add rows to DataTable
                foreach (PriceProduct priceProductType in product.PriceProducts!)
                {
                    DataRow row = priceProductDT.NewRow();

                    row[nameof(PriceProduct.ProductId)] = priceProductType.ProductId;
                    row[nameof(PriceProduct.SalesPrice)] = priceProductType.SalesPrice;
                    row[nameof(PriceProduct.PercentageProfit)] = priceProductType.PercentageProfit;
                    row[nameof(PriceProduct.Revenue)] = priceProductType.Revenue;
                    row[nameof(PriceProduct.Wholesale)] = priceProductType.Wholesale;
                    priceProductDT.Rows.Add(row);
                }

                dynamicParameters.Add("priceProductType", priceProductDT.AsTableValuedParameter("[dbo].[PriceProductType]"));

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                int productId = await conn.QuerySingleOrDefaultAsync<int>("UpdateProduct", dynamicParameters, commandType: CommandType.StoredProcedure);
                conn.Dispose();

                return productId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateStockProduct(int idProduct, int stock)
        {
            try
            {
                int productId = 0;
                DynamicParameters parameters = new();
                parameters.Add("idProduct", idProduct);
                parameters.Add("stock", stock);

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                productId = await conn.QueryFirstAsync<int>("UpdateStockProduct", param: parameters, commandType: CommandType.StoredProcedure);
                conn.Dispose();

                return productId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteProduct(int id, int userId)
        {
            try
            {
                DynamicParameters parameters = new();
                parameters.Add("Id", id);
                parameters.Add("UserId", userId);

                using IDbConnection conn = new SqlConnection(_configuration.GetConnectionString("SalePoinDB"));
                conn.Open();
                int productId = await conn.QuerySingleOrDefaultAsync<int>("DeleteProduct", parameters, commandType: CommandType.StoredProcedure);
                conn.Dispose();

                return productId;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}