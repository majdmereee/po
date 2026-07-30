using System;
using System.Linq;
using System.Windows;

namespace RestaurantHR_App.Views
{
    public partial class AddAttendanceWindow : Window
    {
        private readonly AppDbContext _context;

        public AddAttendanceWindow()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadEmployees();
            DpDate.SelectedDate = DateTime.Today; 
        }

        private void LoadEmployees()
        {
            CmbEmployees.ItemsSource = _context.Employees.Where(e => e.IsActive).ToList();
            if (CmbEmployees.Items.Count > 0)
                CmbEmployees.SelectedIndex = 0;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CmbEmployees.SelectedValue == null)
                {
                    MessageBox.Show("الرجاء اختيار الموظف.", "خطأ", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newAttendance = new Attendance
                {
                    EmployeeId = (int)CmbEmployees.SelectedValue,
                    Date = DpDate.SelectedDate ?? DateTime.Today,
                    CheckInTime = TimeSpan.Parse(TxtCheckIn.Text),
                    CheckOutTime = TimeSpan.Parse(TxtCheckOut.Text),
                    OvertimeHours = 0, 
                    IsAbsent = false
                };

                _context.Attendances.Add(newAttendance);
                _context.SaveChanges();
                
                this.DialogResult = true; 
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"تأكد من إدخال الوقت بصيغة صحيحة (مثال: 09:30)\n{ex.Message}", "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
