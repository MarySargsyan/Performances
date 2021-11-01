using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
     public interface IEvalRepository
    {
        int GetCorrentAssesor(string userName);
        List<Evaluations> AllCorrentAssesorEvaluations(string userName);
        Evaluations GetById(int? id);
        void Insert(Evaluations evaluations);
        void Delete(int? id);
        List<User> MyEmployees(string userName);

    }
}
