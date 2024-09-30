using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;
using StaffManagmentNET.ViewModels;

namespace StaffManagmentNET.Services
{
    public class SalaryService : ISalaryRepo
    {
        private readonly DataContext _context;
        public SalaryService(DataContext context)
        {
            _context = context;
        }

        public async Task AddReward(List<RewardVM> vms)
        {
            foreach (var vm in vms)
            {
                var sa = await _context.Salaries.FindAsync(vm.Key, vm.StaffID);
                sa!.Reward = vm.Reward;
                sa.Total += double.Parse(vm.Reward);
                _context.Salaries.Update(sa);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Salary>> GetSalary(int month, int year)
        {
            var result = new List<Salary>();
            var staffs = await _context.Staffs.ToListAsync();

            var key = $"{month}/{year}";
            var salaries = await _context.Salaries.Where(s => s.Key == key).ToListAsync();

            //if (salaries != null && salaries.Count > 0)
            //{
            //    return salaries;
            //}

            foreach (var staff in staffs)
            {
                if (staff.StaffID == "24001") continue;

                var res = new Salary();
                res.Key = key;
                res.StaffID = staff.StaffID;
                res.StaffName = staff.StaffName;
                res.Title = staff.Title;
                res.TotalHour = await _context.TimeSheets
                    .Where(t => t.CheckIn.Month == month && t.CheckIn.Year == year && t.StaffID == staff.StaffID)
                    .SumAsync(t => t.Total);
                res.TotalHour = Math.Round(res.TotalHour, 2);

                var title = await _context.Settings.Where(s => s.Key.Contains(staff.Title)).FirstOrDefaultAsync();
                if (title != null)
                {
                    res.TitleAllowance = title.Value;
                }

                var degree = await _context.Settings.Where(s => s.Key.Contains(staff.Degree)).FirstOrDefaultAsync();
                if (degree != null)
                {
                    res.DegreeAllowance = degree.Value;
                }

                var cert = await _context.Settings.Where(s => s.Key.Contains(staff.Cert)).FirstOrDefaultAsync();
                if (cert != null)
                {
                    res.CertAllowance = cert.Value;
                }

                var general = await _context.Settings.Where(s => s.Type.Contains("chung")).ToListAsync();
                double generalTotal = 0;
                foreach (var t in general)
                {
                    generalTotal += double.Parse(t.Value);
                }

                res.GeneralAllowance = generalTotal.ToString();

                var basic = await _context.Settings.FindAsync("Hệ số lương cơ bản");
                res.Total = double.Parse(basic!.Value) + double.Parse(res.TitleAllowance) +
                    double.Parse(res.DegreeAllowance) + double.Parse(res.CertAllowance) + generalTotal;

                res.Total = Math.Round(res.Total, 2);

                res.Evaluate = await _context.TaskDetails
                    .Where(t => t.StartTime.Month == month && t.StartTime.Year == year)
                    .AverageAsync(t => t.Evaluate);

                res.Evaluate = Math.Round(res.Evaluate, 2);
                result.Add(res);
                try
                {
                    _context.Salaries.Add(res);
                }
                catch
                {
                    continue;
                }
            }

            await _context.SaveChangesAsync();
            return result;
        }
    }
}
