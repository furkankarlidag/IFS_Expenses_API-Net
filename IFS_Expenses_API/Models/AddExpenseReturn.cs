namespace IFS_Expenses_API.Models
{
    public class AddExpenseReturn
    {
        public bool Success { get; set; }
        public string ErrorDescription { get; set; }
        public string ExpenseId { get; set; }
    }
}
