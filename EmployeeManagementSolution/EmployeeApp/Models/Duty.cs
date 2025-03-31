using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeApp.Models
{
    public class Duty
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]                          //атрибут для связки свойств, кстати часть EntityFrameworkCore, показывает фреймворку, что свойства имеют какие-то отношения
        public Employee? Employee { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }

        [Required]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Task? Task { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty; //атрибут Required нужен,чтобы исключить значения NULL из таблицы, а " = string.Empty" создаёт в таких местах пустую строкку "", которая является допустимым значением
    }                                                    //да,я знаю, что можно просто прописать  "string?", но тогда появляется предупреждение "Разыменование вероятной пустой ссылки.", так что юзаю это
}
