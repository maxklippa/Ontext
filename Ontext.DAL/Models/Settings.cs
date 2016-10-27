using System;
using System.ComponentModel.DataAnnotations;
using Ontext.DAL.Identity;
using Ontext.DAL.Models.Base;

namespace Ontext.DAL.Models
{
    public class Settings
    {
        public Settings()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        public int SortType { get; set; }
        public string Language { get; set; }

        [Required]
        public virtual OntextUser User { get; set; }
    }
}