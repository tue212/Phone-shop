using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop.Model
{
    public class Product: INotifyPropertyChanged, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ImportPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int CatId { get; set; }
        public string ImagePath { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
