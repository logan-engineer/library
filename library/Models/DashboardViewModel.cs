
namespace library.Models
{
    public class DashboardViewModel
    {
        public int TotalBooks { get; set; }
        public int? AvailableBooks { get; set; }
        public int? TotalTransactions { get; set; }
        public int OverdueBooks { get; set; }
        public int TotalGenres { get; set; }
        public List<BookLoan>? RecentBookloans { get; set; }
       

        
    }


}
