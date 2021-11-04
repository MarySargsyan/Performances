using ClosedXML.Excel;
using Perfomans.Models;
using Perfomans.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
    public class GroupService : IGroupService
    {
        private readonly IGroupsRepository _repository;

        public GroupService(IGroupsRepository repository)
        {
            _repository = repository;
        }

        public List<Groups> AllGroups() => _repository.AllGroups();

        public List<Parameters> AllParameters() => _repository.AllParameters();

        public List<ParametersGroup> AllParametersGroups() => _repository.AllParametersGroups();

        public void Delete(int? id) => _repository.Delete(id);

        public void EditParametersGroup(ParametersGroup parametersGroup) => _repository.EditParametersGroup(parametersGroup);

        public Groups GetById(int? id) => _repository.GetById(id);

        public ParametersGroup GetGroupParam(int ParametertId, int GroupId) => _repository.GetGroupParam(ParametertId, GroupId);
        public void Insert(Groups groups) => _repository.Insert(groups);

        public Groups Update(Groups groups, int[] selectedItems, int DepId) => _repository.Update(groups, selectedItems, DepId);

        public void WorkbookCreate(XLWorkbook workbook, int DepId) => _repository.WorkbookCreate(workbook, DepId);
    }
}
