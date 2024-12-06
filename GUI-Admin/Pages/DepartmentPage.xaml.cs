using DTO.Models;
using GUI_Admin.ViewModels;

namespace GUI_Admin.Pages
{
    public partial class DepartmentPage : ContentPage
    {
        public DepartmentPage(Department department)
        {
            InitializeComponent();

            BindingContext = new DepartmentViewModel(department);
        }
    }
}
