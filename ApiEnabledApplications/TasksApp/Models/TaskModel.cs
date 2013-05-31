using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TasksApp.Models
{
    [DataContract]
    public class TaskModel
    {
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [DataMember(IsRequired = true)]
        [Display(Name = "Completed")]
        public bool Completed { get; set; }
    }
}