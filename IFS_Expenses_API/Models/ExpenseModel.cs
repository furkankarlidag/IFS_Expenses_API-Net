namespace IFS_Expenses_API.Models
{
    public class ExpenseModel
    {
        public string PersonId { get; set; }
        public string CustomerId { get; set; }
        public string ExpenseCode { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }
        public string ExpenseTypeDb { get; set; }
        public string DocumentNo { get; set; }
        public int Amount { get; set; }
    }
}
