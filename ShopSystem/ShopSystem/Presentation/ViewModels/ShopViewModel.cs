using ShopSystem.Data.Models;
using ShopSystem.Logic.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ShopSystem.Presentation.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private readonly IShopService _shopService;

        public ObservableCollection<User> Users { get; set; } = new();
        public ObservableCollection<CatalogItem> Catalog { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        public ShopViewModel(IShopService shopService)
        {
            _shopService = shopService;
            LoadData();
        }

        private void LoadData()
        {
            foreach (var user in _shopService.GetAllUsers())
                Users.Add(user);

            foreach (var item in _shopService.GetCatalog())
                Catalog.Add(item);
        }
    }
}