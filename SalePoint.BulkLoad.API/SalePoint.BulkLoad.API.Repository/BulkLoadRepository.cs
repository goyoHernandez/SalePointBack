using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalePoint.BulkLoad.API.Primitives;
using SalePoint.BulkLoad.API.Primitives.Interfaces;
using System.Data;

namespace SalePoint.BulkLoad.API.Repository
{
    public class BulkLoadRepository : IBulkLoadRepository
    {
        private readonly IConfiguration _configuration;

        public BulkLoadRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> BulkLoadProducts(IFormFile formFile, int userId)
        {
            List<NewProductType> productTypes = new();

            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                var csvConfig = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
                var csvReader = new CsvReader(reader, csvConfig);

                // Lee el CSV y mapea los datos a una lista de TuModelo
                productTypes = csvReader.GetRecords<NewProductType>().ToList();
            }

            // Convierte la lista a un DataTable
            DataTable dataTable = ListToDataTableNewProducts(productTypes, userId);

            using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
            conn.Open();

            await conn.ExecuteAsync("BulkLoadProducts", new { products = dataTable.AsTableValuedParameter("[dbo].[NewProductType]") }, commandType: CommandType.StoredProcedure);

            return true;
        }

        public async Task<bool> UpgradeBulkLoadProducts(IFormFile formFile, int userId)
        {
            List<UpgradeProductType> productTypes = new();

            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                var csvConfig = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
                var csvReader = new CsvReader(reader, csvConfig);

                // Lee el CSV y mapea los datos a una lista de TuModelo
                productTypes = csvReader.GetRecords<UpgradeProductType>().ToList();
            }

            // Convierte la lista a un DataTable
            DataTable dataTable = ListToDataTableUpgradeProducts(productTypes, userId);

            using SqlConnection conn = new(_configuration.GetConnectionString("SalePoinDB"));
            conn.Open();

            await conn.ExecuteAsync("BulkUpgradeProducts", new { products = dataTable.AsTableValuedParameter("[dbo].[UpgradeProductType]") }, commandType: CommandType.StoredProcedure);

            return true;
        }

        private static DataTable ListToDataTableNewProducts(List<NewProductType> newProductTypes, int userId)
        {
            DataTable dataTable = new();

            // Configura las columnas del DataTable según tu modelo
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("BarCode", typeof(string));
            dataTable.Columns.Add("ExpirationDate", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Stock", typeof(decimal));
            dataTable.Columns.Add("MinimumStock", typeof(decimal));
            dataTable.Columns.Add("PurchasePrice", typeof(decimal));
            dataTable.Columns.Add("UnitMeasureId", typeof(int));
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("DepartmentName", typeof(string));
            dataTable.Columns.Add("RetailSalePrice", typeof(decimal));
            dataTable.Columns.Add("WholeSalePrice", typeof(decimal));
            dataTable.Columns.Add("WholeSaleQuantity", typeof(int));

            foreach (NewProductType newProductType in newProductTypes)
            {
                DataRow row = dataTable.NewRow();

                row[nameof(newProductType.Name)] = newProductType.Name is null ? "" : newProductType.Name;
                row[nameof(newProductType.BarCode)] = newProductType.BarCode;
                row[nameof(newProductType.ExpirationDate)] = newProductType.ExpirationDate;
                row[nameof(newProductType.Description)] = newProductType.Description is null ? "" : newProductType.Description;
                row[nameof(newProductType.Stock)] = decimal.TryParse(newProductType.Stock, out decimal stock) ? stock : 0;
                row[nameof(newProductType.MinimumStock)] = decimal.TryParse(newProductType.MinimumStock, out decimal minimumStock) ? minimumStock : 0;
                row[nameof(newProductType.PurchasePrice)] = decimal.TryParse(newProductType.PurchasePrice, out decimal purchasePrice) ? purchasePrice : 0;
                row[nameof(newProductType.UnitMeasureId)] = int.TryParse(newProductType.UnitMeasureId, out int unitMeasureId) ? unitMeasureId : 0;
                row[nameof(newProductType.UserId)] = userId;
                row[nameof(newProductType.DepartmentName)] = newProductType.DepartmentName;
                row[nameof(newProductType.RetailSalePrice)] = decimal.TryParse(newProductType.RetailSalePrice, out decimal retailSalePrice) ? retailSalePrice : 0;
                row[nameof(newProductType.WholeSalePrice)] = newProductType.WholeSalePrice == null ? DBNull.Value : decimal.TryParse(newProductType.WholeSalePrice, out decimal wholeSalePrice) ? wholeSalePrice : 0;
                row[nameof(newProductType.WholeSaleQuantity)] = newProductType.WholeSaleQuantity == null ? DBNull.Value : decimal.TryParse(newProductType.WholeSaleQuantity, out decimal wholeSaleQuantity) ? wholeSaleQuantity : 0;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private static DataTable ListToDataTableUpgradeProducts(List<UpgradeProductType> upgradeProductTypes, int userId)
        {
            DataTable dataTable = new();

            // Configura las columnas del DataTable según tu modelo
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("ExpirationDate", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("Stock", typeof(decimal));
            dataTable.Columns.Add("PurchasePrice", typeof(decimal));
            dataTable.Columns.Add("UserId", typeof(int));
            dataTable.Columns.Add("RetailSalePrice", typeof(decimal));
            dataTable.Columns.Add("WholeSalePrice", typeof(decimal));
            dataTable.Columns.Add("WholeSaleQuantity", typeof(int));

            foreach (UpgradeProductType upgradeProductType in upgradeProductTypes)
            {
                DataRow row = dataTable.NewRow();

                row[nameof(upgradeProductType.Name)] = upgradeProductType.Name is null ? "" : upgradeProductType.Name;
                row[nameof(upgradeProductType.ExpirationDate)] = upgradeProductType.ExpirationDate;
                row[nameof(upgradeProductType.Description)] = upgradeProductType.Description is null ? "" : upgradeProductType.Description;
                row[nameof(upgradeProductType.Stock)] = decimal.TryParse(upgradeProductType.Stock, out decimal stock) ? stock : DBNull.Value;
                row[nameof(upgradeProductType.PurchasePrice)] = decimal.TryParse(upgradeProductType.PurchasePrice, out decimal purchasePrice) ? purchasePrice : DBNull.Value;
                row[nameof(upgradeProductType.UserId)] = userId;
                row[nameof(upgradeProductType.RetailSalePrice)] = decimal.TryParse(upgradeProductType.RetailSalePrice, out decimal retailSalePrice) ? retailSalePrice : DBNull.Value;
                row[nameof(upgradeProductType.WholeSalePrice)] = upgradeProductType.WholeSalePrice == null ? DBNull.Value : decimal.TryParse(upgradeProductType.WholeSalePrice, out decimal wholeSalePrice) ? wholeSalePrice : DBNull.Value;
                row[nameof(upgradeProductType.WholeSaleQuantity)] = upgradeProductType.WholeSaleQuantity == null ? DBNull.Value : decimal.TryParse(upgradeProductType.WholeSaleQuantity, out decimal wholeSaleQuantity) ? wholeSaleQuantity : DBNull.Value;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}