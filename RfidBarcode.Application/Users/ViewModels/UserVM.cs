namespace RfidBarcode.Application.Users.ViewModels
{
    public class UserVM
    {
        public Int64 Id { get; set; }
        public Int64 EmployeeId { get; set; }
        public string UserName { get; set; } = null!;
        public string EmployeeNip { get; set; } = null!;

        public string EmployeeName { get; set; } = null!;

        public string DepartmentName { get; set; } = null!;

        public string? Password { get; set; }

        public string Role { get; set; } = null!;

    }
}
