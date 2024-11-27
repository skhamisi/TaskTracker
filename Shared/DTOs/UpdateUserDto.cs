using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Shared.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string? Password { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
