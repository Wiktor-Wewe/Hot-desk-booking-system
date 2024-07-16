namespace Hdbs.Transfer.Employees.Data
{
    public class ImportExcelEmployeeResponse
    {
        public int TotalNumber { get; set; } = 0;
        public int ErrorsNumber { get; set; } = 0;
        public int SuccesedNumber { get; set; } = 0;
        public int SkippedNumber { get; set; } = 0;
        public List<string> Errors { get; set; } = [];
    }
}
