using System.Collections.Generic;

namespace RecordBox.Models
{
  public class Genre
  {
    public Genre()
    {
      this.JoinEntities = new HashSet<RecordGenre>();
    }
    public int GenreId { get; set; }
    public string Name { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<RecordGenre> JoinEntities { get; set; }
  }
}