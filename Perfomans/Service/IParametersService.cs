using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
  public  interface IParametersService
    {
        List<Parameters> AllParameters();
        Parameters GetById(int? id);
        void Insert(Parameters parameters);
        void Update(Parameters parameters);
        void Delete(int? id);
    }
}
