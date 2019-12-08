using System;
using System.Collections.Generic;
using System.Text;
using TeisterMask.DataProcessor.ExportDto;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class EmployeeDto
    {
        public string Username { get; set; }

        public ICollection<TasksDto> Tasks { get; set; }
    }
}
