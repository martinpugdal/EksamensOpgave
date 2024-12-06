using DTO.Models;
using GUI_Admin.ViewModels;

namespace GUI_Admin.Pages
{
    public partial class EmployeePage : ContentPage
    {
        public EmployeePage(Employee employee, Department department)
        {
            InitializeComponent();

            BindingContext = new EmployeeViewModel(employee, department);
        }
    }
}
