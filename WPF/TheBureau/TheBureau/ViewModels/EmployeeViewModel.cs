using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TheBureau.Models;
using TheBureau.Repositories;
using TheBureau.Views;
using TheBureau.Views.Controls;

namespace TheBureau.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        private EmployeeRepository _employeeRepository;
        private readonly BrigadeRepository _brigadeRepository;
        
        private ObservableCollection<Employee> _employees;
        private ObservableCollection<Brigade> _employeeBrigades;
        
        private Employee _selectedItem;
        private string _findEmployeesText;
        
        private ICommand _deleteCommand;
        private ICommand _openEditEmployeeWindowCommand;
        private ICommand _openAddEmployeeWindowCommand;
        
        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set { _employees = value; OnPropertyChanged("Employees"); }
        }
        public EmployeeViewModel()
        {
            try
            {
                _employeeRepository = new EmployeeRepository();
                _brigadeRepository = new BrigadeRepository();
                Employees = new ObservableCollection<Employee>(_employeeRepository.GetAll());
                SelectedItem = Employees.First();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при открытии страницы работников");
                infoWindow.ShowDialog();
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??= new RelayCommand(obj =>
                {
                    try
                    {
                        var empl = SelectedItem;
                        if (empl != null)
                        {
                            int clientid = empl.id;
                            _employeeRepository.Delete(clientid);
                            _employeeRepository.SaveChanges();
                            Employees.Remove(SelectedItem);
                            SelectedItem = Employees.First();
                        }
                    }
                    catch (Exception)
                    {
                        InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при удалении работника");
                        infoWindow.ShowDialog();
                    }
                });
            }
        }
        
        public ICommand OpenEditEmployeeWindowCommand =>
            _openEditEmployeeWindowCommand ??= new RelayCommand(OpenEditEmployeeWindow);

        public ICommand OpenAddEmployeeWindowCommand =>
            _openAddEmployeeWindowCommand ??= new RelayCommand(OpenAddEmployeeWindow);

        private void OpenAddEmployeeWindow(object sender)
        {
            try
            {
                AddEmployeeView view = new();
                if (view.ShowDialog() == true)
                {
                    Update();
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при добавлении работника");
                infoWindow.ShowDialog();
            }
        }
        
        private void OpenEditEmployeeWindow(object sender)
        {
            try
            {
                var employeeToEdit = SelectedItem;
                EditEmployeeView window = new(employeeToEdit);
                if (window.ShowDialog() == true)
                {
                    Update();
                    SelectedItem = _employeeRepository.Get(employeeToEdit.id);
                }
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при изменении работника");
                infoWindow.ShowDialog();
            }
        }

        public Employee SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                SetEmployeeBrigade();
                OnPropertyChanged("SelectedItem");
            }
        }
        
        public void Update()
        {
            try
            {
                _employeeRepository = new EmployeeRepository();
                Employees = new ObservableCollection<Employee>(_employeeRepository.GetAll());
                EmployeeBrigade = new ObservableCollection<Brigade>(_brigadeRepository.GetAll());
                SelectedItem = Employees.First();
            }
            catch (Exception)
            {
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Ошибка при обновлении данных");
                infoWindow.ShowDialog();
            }
        }
        
        public ObservableCollection<Brigade> EmployeeBrigade
        {
            get => _employeeBrigades;
            set { _employeeBrigades = value; OnPropertyChanged("EmployeeBrigade"); }
        }
        
        public string FindEmployeeText
        {
            get => _findEmployeesText;
            set
            {
                _findEmployeesText = value;
                Search(_findEmployeesText); 
                OnPropertyChanged("FindEmployeeText");
            }
        }

        void SetEmployeeBrigade()
        {
            if (SelectedItem != null)
            {
                EmployeeBrigade = new ObservableCollection<Brigade>(_brigadeRepository.GetAll().Where(x => x.id == SelectedItem.brigadeId));
            }
        }
        
        private void Search(string criteria)
        {
            try
            {
                Employees = new ObservableCollection<Employee>(_employeeRepository.FindEmployeesByCriteria(criteria));
                SelectedItem = Employees.First();
            }
            catch (Exception)
            {
                EmployeeBrigade = new ObservableCollection<Brigade>();
                InfoWindow infoWindow = new InfoWindow("Ошибка", "Не удалось отобразить работников");
                infoWindow.ShowDialog();
            }
        }
    }
}