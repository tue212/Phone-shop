using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using PhoneShop.DataAccess;
using PhoneShop.Model;
using PhoneShop.Service;
using PhoneShop.View.Form;
using Windows.Networking.Sockets;

namespace PhoneShop.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private Product? selectedProduct;

        public RelayCommand DeleteSelectedProductCommand { get; }
        public RelayCommand UpdateSelectedProductCommand { get; }
        public RelayCommand AddProductCommand { get; }
        public ObservableCollection<Product> Products { get; set; } = new();
        public Product? SelectedProduct { get => selectedProduct; set
            { 
                selectedProduct = value; 
                DeleteSelectedProductCommand.RaiseCanExecuteChanged();
                UpdateSelectedProductCommand.RaiseCanExecuteChanged();
            }
        }
        public bool HasSelection => SelectedProduct != null;
        public bool NoSelection => SelectedProduct == null;

        ProductService _productService = new ProductService();
        public PagingMetadata Pagination { get; set; } = new();
        public RelayCommand ChangedPageCommand { get; }
        private string _statusMessage = string.Empty;
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ProductViewModel() {
            DeleteSelectedProductCommand = new RelayCommand(
                _ => _deleteSelectedProduct(),
                _ => HasSelection);
            UpdateSelectedProductCommand = new RelayCommand(
                _ => _updateSelectedProduct(),
                _ => HasSelection);
            AddProductCommand = new RelayCommand(
                _ => _addProduct(),
                _ => true);
            ChangedPageCommand = new RelayCommand(
                newPage => _changePage((int?) newPage),
                _ => true);
            _ = _loadProduct();
        }
        void _changePage(int? pageNumber)
        {
            Pagination.PageNumber = pageNumber ?? 1;
            _ = _loadProduct();
        }
        private async Task _loadProduct()
        {
            try
            {
                StatusMessage = "Đang kiểm tra kết nối...";

                var ok = await _productService.TestConnectionAsync();

                if (!ok)
                {
                    StatusMessage = "Kết nối DB thất bại";
                    return;
                }

                    StatusMessage = "Kết nối DB thành công, đang tải dữ liệu...";
                    //Tải data
                    var result = await _productService.GetAll(new PagingRequest()
                    {
                        PageNumber = Pagination.PageNumber,
                        PageSize = Pagination.PageSize
                    });

                    Pagination = result.Pagination;
                    Products.Clear();
                    foreach (var item in result.Item!)
                    {
                        Products.Add(item);
                    }
                    if (Products.Count > 0)
                        SelectedProduct = Products[0];
                    else
                        SelectedProduct = null;

                    StatusMessage = $"Kết nối OK, đã tải {Products.Count} sản phẩm";
            }
     
            catch (Exception ex)
            {
                StatusMessage = "Lỗi: " + ex.Message;
            }
            
        }

        public async void _deleteSelectedProduct()
        {
            if (SelectedProduct == null) return;
            var dialog = new ContentDialog()
            {
                XamlRoot = LoginWindow.ActiveWindow!.Content.XamlRoot,
                Title = $"Bạn có chắc muốn xóa sản phẩm {SelectedProduct.Name} không?",
                PrimaryButtonText = "Đồng ý xóa",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary
            };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Products.Remove(SelectedProduct);
                SelectedProduct = null;
                var info = new ContentDialog()
                {
                    XamlRoot = LoginWindow.ActiveWindow!.Content.XamlRoot,
                    Title = "Xóa sản phẩm thành công",
                    CloseButtonText = "Đóng",
                };
                _ = await info.ShowAsync();
            }


        }

        public async void _updateSelectedProduct()
        {
            if (SelectedProduct == null) return;
            var dialog = new EditProductForm(SelectedProduct)
            {
                XamlRoot = LoginWindow.ActiveWindow!.Content.XamlRoot,
                Title = "Cập nhật sản phẩm ",
                PrimaryButtonText = "Cập nhật",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                _productService.Update(dialog.EditItem);
                var info = new ContentDialog()
                {
                    XamlRoot = LoginWindow.ActiveWindow!.Content.XamlRoot,
                    Title = "Cập nhật sản phẩm thành công",
                    CloseButtonText = "Đóng",
                };
                _ = await info.ShowAsync();
            }
        }

        internal async void _addProduct()
        {
            var dialog = new AddProductForm()
            {
                XamlRoot = LoginWindow.ActiveWindow!.Content.XamlRoot,
                Title = "Thêm sản phẩm mới",
                PrimaryButtonText = "Thêm",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                _productService.Add(dialog.NewItem);
                var info = new ContentDialog()
                {
                    XamlRoot = LoginWindow.ActiveWindow!.Content.XamlRoot,
                    Title = "Thêm sản phẩm thành công",
                    CloseButtonText = "Đóng",
                };
                _ = await info.ShowAsync();
            }

        }
    }
}
