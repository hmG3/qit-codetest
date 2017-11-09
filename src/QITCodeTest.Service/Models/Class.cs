using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace QITCodeTest.Service.Models
{
    public class Class
    {
        public Class()
        {
            Students = new HashSet<Student>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [StringLength(100)]
        public string Teacher { get; set; }

        [JsonIgnore]
        public ICollection<Student> Students { get; set; }

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
            var other = (Class) obj;
            return string.Equals(Name, other.Name) && string.Equals(Location, other.Location);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Name?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (Location?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}