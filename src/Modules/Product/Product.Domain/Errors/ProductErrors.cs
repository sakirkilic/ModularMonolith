using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Errors
{
    // Product modülüne ait domain hataları
    public static class ProductErrors
    {
        public static readonly Error NameEmpty = new(
            "Product.Name.Empty",
            "Ürün adı boş olamaz. / Product name cannot be empty.");

        public static readonly Error NameTooLong = new(
            "Product.Name.TooLong",
            "Ürün adı 200 karakterden uzun olamaz. / Product name cannot be longer than 200 characters.");
    }
}
