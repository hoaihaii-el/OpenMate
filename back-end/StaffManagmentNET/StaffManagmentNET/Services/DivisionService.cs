using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using System.Numerics;
using System.Text.RegularExpressions;

namespace StaffManagmentNET.Services
{
    public class DivisionService : IDivisionRepo
    {
        private readonly DataContext _context;

        public DivisionService(DataContext context)
        {
            _context = context;
        }

        public async Task<Division> CreateDivision(string divisionName, string managerID)
        {
            var division = new Division()
            {
                DivisionID = await AutoID(),
                DivisionName = divisionName,
                ManagerID = managerID
            };

            _context.Divisions.Add(division);
            await _context.SaveChangesAsync();

            return division;
        }

        public async Task<Division> DeleteDivision(string divisionID)
        {
            var division = await _context.Divisions.FindAsync(divisionID);

            if (division == null)
            {
                throw new ArgumentNullException("Division not found");
            }

            _context.Divisions.Remove(division);
            await _context.SaveChangesAsync();

            return division;
        }

        public async Task<Division> UpdateManager(string divisionID, string managerID)
        {
            var division = await _context.Divisions.FindAsync(divisionID);

            if (division == null)
            {
                throw new ArgumentNullException("Division not found");
            }

            division.ManagerID = managerID;

            _context.Divisions.Update(division);
            await _context.SaveChangesAsync();
            return division;
        }

        private async Task<string> AutoID()
        {
            var ID = "DV" + "001";

            var maxID = await _context.Staffs
                .OrderByDescending(s => s.StaffID)
                .Select(v => v.StaffID)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maxID))
            {
                return ID;
            }

            var numeric = Regex.Match(maxID, @"\d+").Value;

            if (string.IsNullOrEmpty(numeric))
            {
                return ID;
            }

            ID = "DV";

            numeric = (int.Parse(numeric) + 1).ToString();

            while (ID.Length + numeric.Length < 6)
            {
                ID += '0';
            }

            return ID + numeric;
        }
    }
}
