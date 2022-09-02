using Apache.Infrastructure.Commands;
using Apache.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Apache.ViewModels
{
    public class OstatokPageViewModel : BaseViewModel
    {
        public OstatokPageViewModel()
        {
            PreviousPageProductCommand = new RelayCommand(PreviousPageProductCommandExecute);
            NextPageProductCommand = new RelayCommand(NextPageProductCommandExecute);

            LoadProducts();

        }
        public ICommand PreviousPageProductCommand { get; set; }
        public ICommand NextPageProductCommand { get; set; }

        private int _currentPageIndexProduct = 1;
        public int CurrentPageIndexProduct
        {
            get => _currentPageIndexProduct;
            set
            {
                Set(ref _currentPageIndexProduct, value);
            }
        }

        private int _pageCountProduct = 1;
        public int PageCountProduct
        {
            get => _pageCountProduct;
            set
            {
                Set(ref _pageCountProduct, value);
            }
        }
        private ObservableCollection<OstatokModel> _productsCollection = new ObservableCollection<OstatokModel>();
        public ObservableCollection<OstatokModel> ProductCollection
        {
            get => _productsCollection;
            set
            {
                Set(ref _productsCollection, value);
            }
        }

        private async void PreviousPageProductCommandExecute(object o)
        {
            await using (ApplicationContext db = new())
            {
                if (CurrentPageIndexProduct == 1)
                    return;
                CurrentPageIndexProduct--;
                ProductCollection = new ObservableCollection<OstatokModel>(ReturnTrueProduct());
            }
        }
        private async void NextPageProductCommandExecute(object o)
        {
            await using (ApplicationContext db = new ApplicationContext())
            {
                if (CurrentPageIndexProduct == PageCountProduct)
                {
                    return;
                }
                else if (CurrentPageIndexProduct > PageCountProduct)
                {
                    CurrentPageIndexProduct = 1;
                    LoadProducts();
                    return;
                }
                CurrentPageIndexProduct++;
                ProductCollection = new ObservableCollection<OstatokModel>(ReturnTrueProduct());
            }
        }

        private List<OstatokModel> ReturnTrueProduct() 
        {
            using ApplicationContext db = new ApplicationContext();

            List<OstatokModel> ostatok = db.ProductsPrihod!.Select(s=> new OstatokModel {Id= s.ProductsId,Name = db.Products!.First(a=>a.Id == s.ProductsId).Name!,Category = db.Categories!.First(c=>c.Id == db.Products!.First(p=>p.Id == s.ProductsId).CategoryId).Name,Count = s.ComeQuantity}).ToList();

            var result = ostatok.GroupBy(x => x.Name).Select(s => new OstatokModel {Name = s.Key, Count = s.Sum(x=>x.Count)}).ToList();

            PageCountProduct = Convert.ToInt32(Math.Ceiling(result.Count() / 16m));

            return result.Skip((CurrentPageIndexProduct -1 )*16).Take(16).ToList();
        }

        private async void LoadProducts()
        {
            await using (ApplicationContext db = new())
            {
                ProductCollection = new ObservableCollection<OstatokModel>(ReturnTrueProduct());
            }
        }
    }
}
