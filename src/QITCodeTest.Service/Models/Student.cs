using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace QITCodeTest.Service.Models
{
    public class Student
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        public DateTime DOB { get; set; }

        [Range(1.0, 5.0)]
        public double? GPA { get; set; }

        public Guid ClassId { get; set; }

        [JsonIgnore]
        public Class Class { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            var other = (Student)obj;
            return string.Equals(Name, other.Name) && string.Equals(Surname, other.Surname);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Surname?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}