using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Perfomans.Repository
{
    public class EvalRepository : IEvalRepository
    {

        private readonly ApplicationContext _context;

        public EvalRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<Evaluations> AllCorrentAssesorEvaluations(string userName)
        {
            var applicationContext = _context.Evaluations.Where(e => e.AssessorId == GetCorrentAssesor(userName) ||
            e.Assessor.SupervisorId == GetCorrentAssesor(userName)).Include(u => u.Assessor);
            return applicationContext.ToList();
        }

        public List<Parameters> AllParam()
        {
            return _context.Parameters.ToList();
        }

        public void Delete(int? id)
        {
            Evaluations eval = _context.Evaluations.Find(id);
            _context.Evaluations.Remove(eval);
            _context.SaveChanges();
        }

        public Evaluations GetById(int? id)
        {
            return _context.Evaluations.Include(u => u.Assessor).Include(u=> u.UserParamEvals).FirstOrDefault(m => m.Id == id);           
        }

        public int GetCorrentAssesor(string userName)
        {
            int CorrentUserId = 0;
            foreach (User user in _context.User.ToList())
            {
                if (userName == user.Email)
                {
                    CorrentUserId = user.Id;
                }
            }
            return (CorrentUserId);
        }

        public void Insert(Evaluations evaluations)
        {
            _context.Add(evaluations);

            foreach (User user in _context.User.Where(u => u.SupervisorId == evaluations.AssessorId))
            {
                foreach (Parameters parameters in _context.Parameters.ToList())
                {
                    UserParamEval userParamEval = new UserParamEval();
                    userParamEval.EvaluationsId = evaluations.Id;
                    userParamEval.UserId = user.Id;
                    userParamEval.ParameterId = parameters.Id;
                    userParamEval.Mark = 0;
                    _context.UserParamEval.Add(userParamEval);
                }
            }
            _context.SaveChanges();
        }

        public List<User> MyEmployees(string userName)
        {
            List<User> myEmployees = new List<User>();

            foreach (User users in _context.User.Where(u => u.SupervisorId == GetCorrentAssesor(userName)).ToList())
            {
                myEmployees.Add(users);
                foreach (User u in _context.User.Where(u => u.SupervisorId == users.Id).ToList())
                {
                    myEmployees.Add(u);
                }
            }
            return myEmployees;
        }

        public void Update(Evaluations evaluations, List<int> marks)
        {
            foreach (UserParamEval userParamEval in _context.UserParamEval.Where(upe => upe.EvaluationsId == evaluations.Id))
            {
                for (int i = 0; i < marks.Count; i++)
                {
                    userParamEval.Mark = marks[i];
                    marks.Remove(marks.First());
                    break;
                }
            }
            _context.SaveChanges();
        }
    }
}