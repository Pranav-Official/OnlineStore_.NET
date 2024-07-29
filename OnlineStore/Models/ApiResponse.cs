namespace OnlineStore.Models
{
    public class ApiResponse<T>
    {
        public string Status { get; set; }
        public T Data { get; set; }
        public int? Count { get; set; } 
        public string Error { get; set; }
    }
}
