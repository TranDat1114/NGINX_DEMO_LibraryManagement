using System.ComponentModel.DataAnnotations;

namespace LibraryManagement;
public class Book
{
    [Key]
    public int Isbn { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
}