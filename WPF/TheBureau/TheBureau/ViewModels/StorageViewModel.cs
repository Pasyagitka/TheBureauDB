using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TheBureau.Annotations;
using TheBureau.Models;
using TheBureau.Repositories;

namespace TheBureau.ViewModels
{
    public class StorageViewModel : ViewModelBase
    {
        private readonly ToolRepository _toolRepository = new();
        private readonly AccessoryRepository _accessoryRepository = new();
        private readonly EquipmentRepository _equipmentRepository = new();

        private ObservableCollection<Tool> _tools;
        private ObservableCollection<Accessory> _accessories;
        private ObservableCollection<Equipment> _equipments;

        public ObservableCollection<Tool> Tools 
        { 
            get => _tools; 
            set  {  _tools = value;  OnPropertyChanged("Tools"); } 
        }
        public ObservableCollection<Accessory> Accessories 
        { 
            get => _accessories; 
            set  {  _accessories = value;  OnPropertyChanged("Accessories");  } 
        }

        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set { _equipments = value;  OnPropertyChanged("Equipments");  }
        }

        public StorageViewModel()
        {
            Tools = new ObservableCollection<Tool>(_toolRepository.GetAll());
            Accessories = new ObservableCollection<Accessory>(_accessoryRepository.GetAll());
            Equipments = new ObservableCollection<Equipment>(_equipmentRepository.GetAll());
        }
    }
}