using System.ComponentModel.DataAnnotations.Schema;

namespace NotUyg.Entity
{
    [Table("Tag")]
    public class Tag
    {
        public Tag()
        {
            Nots= new List<Not>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Not> Nots { get; set; }
    }
}
