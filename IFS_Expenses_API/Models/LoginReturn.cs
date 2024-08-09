namespace IFS_Expenses_API.Models
{
    public class LoginReturn
    {
        public bool Success { get; set; }
        public string ErrorDescription { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
