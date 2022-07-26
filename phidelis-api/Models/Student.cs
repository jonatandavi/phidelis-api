using System;

namespace phidelis_api.Models
{
    public class Student
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerat‌ed(System.ComponentM‌​odel.DataAnnotations‌​.Schema.DatabaseGeneratedOp‌​tion.None)]
        public int Registration { get; set; }
        public string Name { get; set; }
        public string DocNumber { get; set; }
        public DateTime DateTimeRegistration { get; set; }
    }

}
