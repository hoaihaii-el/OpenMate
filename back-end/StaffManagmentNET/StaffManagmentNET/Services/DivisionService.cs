using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;

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
                DivisionID = divisionName.ToUpper(),
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
    }
}
