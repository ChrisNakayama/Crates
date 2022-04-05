using System.Collections.Generic;

namespace RecordBox.Models
{
  public class Record
  {
    public Record()
    {
      this.JoinEntities = new HashSet<RecordGenre>();
    }
    public int RecordId { get; set; }
    public string Name { get; set; }
    public string Ingredients { get; set; }
    public string Instructions { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<RecordGenre> JoinEntities { get; set; }
  }
}