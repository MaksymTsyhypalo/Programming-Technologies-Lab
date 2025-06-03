// ShopSystem/Presentation/ViewModels/ShopViewModel.cs
using ShopSystem.Data.Models;
using ShopSystem.Logic.Interfaces;
using ShopSystem.Presentation.Commands;
using System.Collections.Generic; // Added for Dictionary
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq; // Added for LINQ operations
using System.Windows.Input;
using System.Windows;

namespace ShopSystem.Presentation.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private readonly IShopService _shopService;
        private User _selectedUser;
        private CatalogItem _selectedItem;
        private int _purchaseQuantity = 1; // New property for quantity

        public ObservableCollection<User> Users { get; } = new();
        public ObservableCollection<CatalogItem> Catalog { get; } = new();
        public ObservableCollection<Event> RecentEvents { get; } = new();
        public ObservableCollection<InventoryDisplayItem> Inventory { get; } = new(); // Changed to a helper class for display

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                ((RelayCommand)PurchaseCommand).RaiseCanExecuteChanged(); // Notify command can execute changed
            }
        }

        public CatalogItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                ((RelayCommand)PurchaseCommand).RaiseCanExecuteChanged(); // Notify command can execute changed
            }
        }

        public int PurchaseQuantity
        {
            get => _purchaseQuantity;
            set
            {
                if (_purchaseQuantity != value)
                {
                    _purchaseQuantity = value;
                    OnPropertyChanged(nameof(PurchaseQuantity));
                    ((RelayCommand)PurchaseCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand PurchaseCommand { get; }
        public ICommand RefreshInventoryCommand { get; }
        public ICommand AddUserCommand { get; } // Example: Add new commands for Master-Detail (CRUD)
        public ICommand AddCatalogItemCommand { get; } // Example: Add new commands for Master-Detail (CRUD)

        public event PropertyChangedEventHandler PropertyChanged;

        public ShopViewModel(IShopService shopService)
        {
            _shopService = shopService;

            LoadDataCommand = new RelayCommand(LoadData);
            PurchaseCommand = new RelayCommand(PurchaseItem, CanPurchase);
            RefreshInventoryCommand = new RelayCommand(RefreshInventory);
            AddUserCommand = new RelayCommand(AddNewUser); // Placeholder for new user logic
            AddCatalogItemCommand = new RelayCommand(AddNewCatalogItem); // Placeholder for new catalog item logic

            // Ensure initial state exists when the application starts
            _shopService.InitializeShopState();
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
            RefreshRecentEvents(); // Also refresh events
        }

        private void RefreshInventory()
        {
            Inventory.Clear();
            foreach (var entry in _shopService.GetInventory())
                Inventory.Add(new InventoryDisplayItem(entry.Key, entry.Value));
        }

        private void RefreshRecentEvents()
        {
            RecentEvents.Clear();
            // Assuming GetRecentEvents exists in IShopService, though it's in IDataRepository
            // For a proper Logic layer, this should be exposed by IShopService.
            // For now, let's assume _shopService can retrieve them.
            // _shopService.GetRecentEvents() would be ideal. If not, expose it via ShopService.
            foreach (var ev in _shopService.GetAllEvents().OrderByDescending(e => e.Timestamp).Take(10)) // Taking recent 10 events for display
                RecentEvents.Add(ev);
        }

        private void PurchaseItem()
        {
            if (CanPurchase())
            {
                try
                {
                    _shopService.ProcessPurchase(SelectedUser.Id, SelectedItem.Id, PurchaseQuantity);
                    RefreshInventory();
                    RefreshRecentEvents();
                    // Optionally, clear selection or provide feedback
                    SelectedItem = null;
                    PurchaseQuantity = 1;
                }
                catch (System.InvalidOperationException ex)
                {
                    // Handle business logic errors (e.g., insufficient stock)
                    System.Windows.MessageBox.Show(ex.Message, "Purchase Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
            }
        }

        private bool CanPurchase() => SelectedUser != null && SelectedItem != null && PurchaseQuantity > 0;

        // Placeholder methods for new CRUD operations to demonstrate Master-Detail interaction
        private void AddNewUser()
        {
            // Logic to open a dialog or set properties for a new user
            // Example: var newUser = new Customer { Name = "New Customer" };
            // _shopService.AddUser(newUser);
            // LoadData();
        }

        private void AddNewCatalogItem()
        {
            // Logic to open a dialog or set properties for a new item
            // Example: var newItem = new ConcreteCatalogItem { Name = "New Item", Price = 0.0m };
            // _shopService.AddCatalogItem(newItem);
            // LoadData();
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Helper class for displaying Inventory items with quantity
    public class InventoryDisplayItem : ViewModelBase
    {
        public CatalogItem Item { get; }
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); }
        }

        public string DisplayName => $"{Item?.Name} (Qty: {Quantity})";

        public InventoryDisplayItem(CatalogItem item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}