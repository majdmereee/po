using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace RestaurantHR_App
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public decimal OvertimeHours { get; set; }
        public bool IsAbsent { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=RestaurantHR.db");
        }
    }

    public class ExportService
    {
        public void ExportToExcel(IEnumerable<Attendance> attendances, string filePath)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("سجل الدوام");
            
            worksheet.Cell(1, 1).Value = "التاريخ";
            worksheet.Cell(1, 2).Value = "الموظف";
            worksheet.Cell(1, 3).Value = "وقت الدخول";
            worksheet.Cell(1, 4).Value = "وقت الخروج";
            worksheet.Cell(1, 5).Value = "إضافي (ساعات)";

            var header = worksheet.Range("A1:E1");
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E293B");
            header.Style.Font.FontColor = XLColor.White;

            int row = 2;
            foreach (var record in attendances)
            {
                worksheet.Cell(row, 1).Value = record.Date.ToString("yyyy-MM-dd");
                worksheet.Cell(row, 2).Value = record.Employee?.Name;
                worksheet.Cell(row, 3).Value = record.CheckInTime.ToString(@"hh\:mm");
                worksheet.Cell(row, 4).Value = record.CheckOutTime?.ToString(@"hh\:mm") ?? "-";
                worksheet.Cell(row, 5).Value = record.OvertimeHours;
                row++;
            }
            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(filePath);
        }
    }
}
