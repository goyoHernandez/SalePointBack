namespace SalePoint.Primitives.Interfaces
{
    public interface ITicketRepository
    {
        Task CreateTicket(Ticket ticket);

        Task<Ticket?> GetTicket();
    }
}