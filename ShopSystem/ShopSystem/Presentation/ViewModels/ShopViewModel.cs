using ShopSystem.Data.Models;
using ShopSystem.Logic.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ShopSystem.Presentation.Commands;

namespace ShopSystem.Presentation.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private readonly IShopService _shopService;
        private User _selectedUser;
        private CatalogItem _selectedItem;

        public ObservableCollection<User> Users { get; } = new();
        public ObservableCollection<CatalogItem> Catalog { get; } = new();
        public ObservableCollection<Event> RecentEvents { get; } = new();
        public ObservableCollection<CatalogItem> Inventory { get; } = new();

        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }

        public CatalogItem SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand PurchaseCommand { get; }
        public ICommand RefreshInventoryCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ShopViewModel(IShopService shopService)
        {
            _shopService = shopService;

            LoadDataCommand = new RelayCommand(LoadData);
            PurchaseCommand = new RelayCommand(PurchaseItem, CanPurchase);
            RefreshInventoryCommand = new RelayCommand(RefreshInventory);

            LoadData();
        }

        private void LoadData()
        {
            Users.Clear();
            foreach (var user in _shopService.GetAllUsers())
                Users.Add(user);

            Catalog.Clear();
            foreach (var item in _shopService.GetCatalog())
                Catalog.Add(item);

            RefreshInventory();
        }

        private void RefreshInventory()
        {
            Inventory.Clear();
            foreach (var item in _shopService.GetInventory())
                Inventory.Add(item);
        }

        private void PurchaseItem()
        {
            if (SelectedUser != null && SelectedItem != null)
            {
                _shopService.ProcessPurchase(SelectedUser.Id, SelectedItem.Id);
                RefreshInventory();
            }
        }

        private bool CanPurchase() => SelectedUser != null && SelectedItem != null;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}