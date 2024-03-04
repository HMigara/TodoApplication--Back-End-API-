using System.ComponentModel.DataAnnotations;

namespace TodoApplication.Modles
{
    public class TodoModel
    {
        [Key]
        public int id { get; set; }
        public string? titel { get; set; }
        public string? discription { get; set; }
        public DateTime setDate { get; set; }
        public DateTime endDate { get; set; }
    }
}
