using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
    public class ParametersRepository : IParametersRepository
    {
        public ApplicationContext _context;

        public ParametersRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<Parameters> AllParameters()
        {
            using (_context)
            {
                return _context.Parameters.ToList();
                 
            }
        }

        public void Delete(int? id)
        {
            Parameters parameters = _context.Parameters.Find(id);
            _context.Parameters.Remove(parameters);
            _context.SaveChanges();
        }

        public Parameters GetById(int? id)
        {
            return _context.Parameters.Find(id);
        }

        public void Insert(Parameters parameters)
        {
            _context.Add(parameters);
            _context.SaveChanges();
        }

        public void Update(Parameters parameters)
        {
            _context.Parameters.Update(parameters);
            _context.SaveChanges();
        }
    }
}
