using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
    public interface IParametersRepository
    {
        List<Parameters> AllParameters();
        Parameters GetById(int? id);
        void Insert(Parameters parameters);
        void Update(Parameters parameters);
        void Delete(int? id);
    }
}
