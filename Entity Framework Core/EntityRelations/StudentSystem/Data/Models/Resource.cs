using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourseId { get; set; }

        [MaxLength(50)]
        [Column(TypeName = "NVARCHAR(50)")]
        public string Name { get; set; }

        public string Url { get; set; }

        public ResourseType ResourseType { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
