using ClosedXML.Excel;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Service
{
    public interface IGroupService
    {
        List<Groups> AllGroups();
        List<Parameters> AllParameters();
        List<ParametersGroup> AllParametersGroups();
        Groups GetById(int? id);
        void Insert(Groups groups);
        Groups Update(Groups groups, int[] selectedItems, int DepId);
        void Delete(int? id);
        void WorkbookCreate(XLWorkbook workbook, int DepId);
        ParametersGroup GetGroupParam(int ParametertId, int GroupId);
        void EditParametersGroup(ParametersGroup parametersGroup);
    }
}
