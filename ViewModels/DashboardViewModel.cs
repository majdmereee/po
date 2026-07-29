using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RestaurantHR_App.ViewModels
{
    public class DashboardViewModel
    {
        private readonly AppDbContext _context;
        private readonly ExportService _exportService;

        public int TotalEmployees { get; set; }
        public int PresentToday { get; set; }
        public int AbsentToday { get; set; }
        public ObservableCollection<Attendance> FilteredAttendances { get; set; }

        public DashboardViewModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
            _exportService = new ExportService();
            FilteredAttendances = new ObservableCollection<Attendance>();

            SeedInitialData();
            LoadStats();
            ApplyFilters("", 0, null, null); 
        }

        private void SeedInitialData()
        {
            if (!_context.Employees.Any())
            {
                var emp1 = new Employee { Name = "أحمد محمود", Position = "شيف رئيسي", BasicSalary = 1200 };
                var emp2 = new Employee { Name = "سارّة خالد", Position = "مديرة صالة", BasicSalary = 1000 };
                _context.Employees.AddRange(emp1, emp2);
                
                _context.Attendances.AddRange(
                    new Attendance { EmployeeId = 1, Date = DateTime.Today, CheckInTime = new TimeSpan(9, 0, 0), CheckOutTime = new TimeSpan(17, 0, 0) },
                    new Attendance { EmployeeId = 2, Date = DateTime.Today, CheckInTime = new TimeSpan(9, 15, 0), CheckOutTime = new TimeSpan(17, 0, 0) }
                );
                _context.SaveChanges();
            }
        }

        public void LoadStats()
        {
            TotalEmployees = _context.Employees.Count(e => e.IsActive);
            var today = DateTime.Today;
            PresentToday = _context.Attendances.Count(a => a.Date == today && !a.IsAbsent);
            AbsentToday = _context.Attendances.Count(a => a.Date == today && a.IsAbsent);
        }

        public void ApplyFilters(string searchText, int dateFilterIndex, DateTime? start, DateTime? end)
        {
            var query = _context.Attendances.Include(a => a.Employee).AsQueryable();
            DateTime today = DateTime.Today;

            if (dateFilterIndex == 0) query = query.Where(a => a.Date == today);
            else if (dateFilterIndex == 1) query = query.Where(a => a.Date >= today.AddDays(-7));
            else if (dateFilterIndex == 2) query = query.Where(a => a.Date >= today.AddDays(-30));
            else if (dateFilterIndex == 3 && start.HasValue && end.HasValue) 
                query = query.Where(a => a.Date >= start.Value && a.Date <= end.Value);

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(a => a.Employee.Name.Contains(searchText));

            FilteredAttendances.Clear();
            foreach (var item in query.OrderByDescending(a => a.Date).ToList())
                FilteredAttendances.Add(item);
        }

        public void ExportToExcel(string path) => _exportService.ExportToExcel(FilteredAttendances, path);
    }
}
