
namespace SalePoint.Primitives.Interfaces
{
    public interface IMeasurementUnitRepository
    {
        Task<IEnumerable<MeasurementUnit>> GetMeasurementUnit();
    }
}