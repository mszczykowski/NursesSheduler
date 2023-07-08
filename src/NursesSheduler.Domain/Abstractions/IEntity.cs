using System.ComponentModel.DataAnnotations;

namespace NursesScheduler.Domain.Abstractions
{
    public record IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
