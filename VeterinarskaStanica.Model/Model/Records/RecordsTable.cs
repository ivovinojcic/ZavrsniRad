using System;
using VeterinarskaStanica.Model.Core;

namespace VeterinarskaStanica.Model.Model.Records
{
    public class RecordsTable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PetName { get; set; }
        public string PetType { get; set; }
        public string EmployeeName { get; set; }
        public string RecordStatus { get; set; }
        public int RecordStatusId { get; set; }
        public DateTime Date { get; set; }

        public RecordsTable(VisitRecord record)
        {
            Id = record.Id;
            UserId = record.Pet.UserId??0;
            UserName = $"{record.Pet.User.Name} {record.Pet.User.Surname}";
            PetName = record.Pet.Name;
            PetType = record.Pet.PetType.Name;
            EmployeeName = $"{record.Employee.Name} {record.Employee.Surname}";
            RecordStatusId = record.RecordStatusId??0;
            RecordStatus = record.RecordStatus.Name;
            Date = record.Date ?? new DateTime();
        }
    }
}
