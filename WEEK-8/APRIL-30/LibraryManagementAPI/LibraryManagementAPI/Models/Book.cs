using System;
using System.Collections.Generic;

namespace LibraryManagementAPI.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string Isbn { get; set; } = null!;

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }

    public int TotalCopies { get; set; }

    public int AvailableCopies { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public virtual Category Category { get; set; } = null!;
}
