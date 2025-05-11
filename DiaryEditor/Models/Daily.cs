namespace DiaryEditor.Models;

public class Daily
{
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    
    public DateTime Updated { get; set; }
    public int UserId { get; set; }
    public string? RecordPassword { get; set; }
    public string? JournalEntry { get; set; }
    
}