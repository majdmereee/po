using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using RestaurantHR_App.ViewModels;

namespace RestaurantHR_App.Views
{
    public partial class DashboardView : Window
    {
        private readonly DashboardViewModel _vm;

        public DashboardView()
        {
            InitializeComponent();
            _vm = new DashboardViewModel();
            DataContext = _vm;
        }

        private void Filter_Changed(object sender, RoutedEventArgs e)
        {
            if (_vm == null || CmbDateFilter == null) return;

            bool isCustom = CmbDateFilter.SelectedIndex == 3;
            if(DpStart != null && DpEnd != null)
            {
                DpStart.Visibility = isCustom ? Visibility.Visible : Visibility.Collapsed;
                DpEnd.Visibility = isCustom ? Visibility.Visible : Visibility.Collapsed;
            }

            _vm.ApplyFilters(
                TxtSearch?.Text ?? "", 
                CmbDateFilter.SelectedIndex, 
                DpStart?.SelectedDate, 
                DpEnd?.SelectedDate
            );
        }

        private void OpenAddAttendance_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddAttendanceWindow();
            if (addWindow.ShowDialog() == true)
            {
                _vm.LoadStats(); 
                Filter_Changed(null, null); 
            }
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = "Excel|*.xlsx", FileName = "تقرير_الدوام.xlsx" };
            if (dialog.ShowDialog() == true)
            {
                _vm.ExportToExcel(dialog.FileName);
                MessageBox.Show("تم التصدير بنجاح!", "تصدير", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
