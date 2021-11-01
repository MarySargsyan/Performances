using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Perfomans.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Perfomans.Repository
{
    public class GroupsRepository : IGroupsRepository
    {
        public ApplicationContext _context;

        public GroupsRepository(ApplicationContext context)
        {
            _context = context;
        }

        public List<Groups> AllGroups()
        {
           return _context.Groups.ToList();
        }

        public List<Parameters> AllParameters()
        {
            return _context.Parameters.ToList();
        }

        public List<ParametersGroup> AllParametersGroups()
        {
            return _context.ParametersGroups.ToList();
        }

        public void Delete(int? id)
        {
            Groups groups = _context.Groups.Find(id);
            _context.Groups.Remove(groups);
            _context.SaveChanges();
        }

        public void EditParametersGroup(ParametersGroup parametersGroup)
        {
            _context.Update(parametersGroup);
            _context.SaveChanges();
        }

        public XLWorkbook Excel(int DepId)
        {
            Departments departments = _context.Departments.Find(DepId);
            departments.Groups = _context.Groups.ToList();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Groups");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Parameters";
                foreach (Groups groups in departments.Groups)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = groups.id;
                    worksheet.Cell(currentRow, 2).Value = groups.Name;
                    string Parameters = " ";
                    if (groups.DepartmentId == DepId)
                    {
                        foreach (ParametersGroup groupsParameters in _context.ParametersGroups.ToList())
                        {
                            if (groupsParameters.GroupId == groups.id)
                            {
                                foreach (Parameters parameters in _context.Parameters.ToList())
                                {
                                    if (parameters.Id == groupsParameters.ParameterId)
                                    {
                                        Parameters = Parameters + parameters.Name + "\n";

                                    }
                                }
                            }
                        }
                    }
                    worksheet.Cell(currentRow, 3).Value = Parameters;
                }
                return workbook;
            }

        }

        public Groups GetById(int? id)
        {
            return _context.Groups.Find(id);
        }

        public ParametersGroup GetGroupParam(int ParametertId, int GroupId)
        {
            foreach (ParametersGroup groupsParameters in _context.ParametersGroups.ToList())
            {
                if ((groupsParameters.GroupId == GroupId) & (groupsParameters.ParameterId == ParametertId))
                {
                    return groupsParameters;
                }
            }
            return _context.ParametersGroups.Find(1);
        }

        public void Insert(Groups groups)
        {
            _context.Add(groups);
            _context.SaveChanges();
        }
        public Groups Update(Groups groups, int[] selectedItems, int DepId)
        {
            Groups editgroup = _context.Groups.Find(groups.id);
            editgroup.Name = groups.Name;
            editgroup.DepartmentId = DepId;
            foreach (ParametersGroup parametersGroup in _context.ParametersGroups.Where(p => p.GroupId == groups.id))
            {
                _context.ParametersGroups.Remove(parametersGroup);
            }
            foreach (var i in _context.Parameters.Where(co => selectedItems.Contains(co.Id)))
            {
                ParametersGroup paramGroup = new ParametersGroup()
                {
                    ParameterId = i.Id,
                    GroupId = groups.id,
                    Mark = 5
                };
                _context.ParametersGroups.Add(paramGroup);
            }

            _context.Entry(editgroup).State = EntityState.Modified;
            _context.SaveChanges();
            return editgroup;
        }
    }
}
