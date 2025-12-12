namespace CollegeManagement.Models
{
    public class LibraryCardDTO
    {
        public int Id { get; set; }
        //public string? CardNumber { get; set; } = string.Empty; //should autogenerate
        public int LibraryId { get; set; }

        //public bool HasBorrowedBook { get; set; }
        //public bool HasReturnedBook { get; set; }
        //public bool IsBookAvailable { get; set; }
        //public int BookId { get; set; }
    }
}
