using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;

namespace WinFormsApp1.Repositories
{
    internal class EmployeeRepository :GenericRepository<Employee>
    {
        public EmployeeRepository(CompanyContext companyContext):base(companyContext) { }
        public Employee GetById (int id)
        {
            return _companyContext.Employees.FirstOrDefault(e => e.Id == id);
        }
        public void Update(int id ,Employee new_employee) { 
            Employee existing_emp = GetById(id);
            existing_emp.Name = new_employee.Name;
            existing_emp.Address = new_employee.Address;
            existing_emp.DepartmentId = new_employee.DepartmentId;
            existing_emp.Phone = new_employee.Phone;    
        }
        public void Delete(int id)
        {
            Employee existing_emp = GetById(id);
            _companyContext.Remove(existing_emp);
        }
    }
}
