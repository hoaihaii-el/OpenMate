using Microsoft.EntityFrameworkCore;
using StaffManagmentNET.Models;
using StaffManagmentNET.Repositories;

namespace StaffManagmentNET.Services
{
    public class SettingService : ISettingRepo
    {
        private readonly DataContext _context;

        public SettingService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Setting>> GetAll()
        {
            var result = await _context.Settings.OrderBy(s => s.Type).ToListAsync();
            return result;
        }

        public async Task UpdateAll(List<Setting> settings)
        {
            foreach (var setting in settings)
            {
                var sett = await _context.Settings.FindAsync(setting.Key);

                if (sett == null)
                {
                    _context.Settings.Add(setting);
                    continue;
                }

                sett.Value = setting.Value;
            }

            await _context.SaveChangesAsync();
        }
    }
}
