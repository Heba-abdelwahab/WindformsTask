using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;

namespace WinFormsApp1.Repositories
{
    internal class GenericRepository<T> where T : class
    {
        readonly protected CompanyContext _companyContext;


        public GenericRepository(CompanyContext companyContext)
        {
            _companyContext = companyContext;
        }
        public void Add(T y)
        {
            _companyContext.Set<T>().Add(y);
        }

        public void SaveChanges()
        {
            _companyContext.SaveChanges();
        }
    
    }
}
