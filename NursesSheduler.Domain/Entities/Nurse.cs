using NursesSheduler.Domain.Common;

namespace NursesSheduler.Domain.Entities
{
    public class Nurse : ISoftDelete
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public int? DepartamentId { get; set; }

        private Departament _departament;
        public Departament Departament
        {
            get => _departament;
            set
            {
                DepartamentId = value?.Id;
                _departament = value;
            }
        }

        public bool IsDeleted { get; set; }
    }
}
