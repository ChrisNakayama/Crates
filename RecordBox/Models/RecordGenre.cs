namespace RecordBox.Models
{
  public class RecordGenre
  {
    public int RecordGenreId { get; set; }
    public int RecordId { get; set; }
    public int GenreId { get; set; }
    public virtual Record Record { get; set; }
    public virtual Genre Genre { get; set; }
  }
}