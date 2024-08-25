using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;

namespace WinFormsApp1
{
    internal class TreeNodeData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ParentId { get; set; } 
        public bool IsEmployee { get; set; } 
        public Employee EmployeeDetails { get; set; }
        public Department DepartmentParentDetails {  get; set; }
    }
}
