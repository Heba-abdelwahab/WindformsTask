using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;

namespace WinFormsApp1.Repositories
{
    internal class DepartmentRepository :GenericRepository<Department>
    {
        public DepartmentRepository(CompanyContext companyContext) : base(companyContext) { }
        public List<Department> GetAllDepartments (){  
            return _companyContext.Departments.ToList();
        }
        public List<Department> GetAllDepartmentsWithEmp()
        {
            return _companyContext.Departments.Include(d=>d.Employees) .ToList();
        }
        public Department GetById (int id)
        {
            return _companyContext.Departments.FirstOrDefault(d => d.Id == id);
        }
        public void Update(int id , Department newDept)
        {
            Department existDept = GetById(id);
            existDept.Name = newDept.Name;
        }
        public void Delete(int id)
        {
            Department existDept = GetById(id);
            _companyContext.Remove(existDept);
        }
    }
}
