using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Models.ViewModels
{
    public class GradeCreateViewModel
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;
        public string? ClassName { get; set; }

        [Display(Name = "Дата урока")]
        [DataType(DataType.Date)]
        public DateTime LessonDate { get; set; } = DateTime.Today;

        [Display(Name = "Предмет")]
        [Required]
        public int SubjectId { get; set; }

        [Display(Name = "Оценка")]
        [Range(1, 5, ErrorMessage = "Оценка от 1 до 5")]
        public int Value { get; set; }

        [Display(Name = "Комментарий")]
        public string? Comment { get; set; }

        // список для выпадающего списка
        public List<SelectListItem> Subjects { get; set; } = new();
        public int? GradeId { get; set; }
    }
}
