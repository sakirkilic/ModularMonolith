using BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Errors
{
    // Product domain hatalarını merkezi olarak tutar
    public static class ProductErrors
    {
        // Ürün adı boş olamaz
        public static readonly Error NameEmpty =
            new("Product.NameEmpty", "Ürün adı boş olamaz.");

        // Ürün fiyatı sıfırdan büyük olmalıdır
        public static readonly Error PriceMustBeGreaterThanZero =
            new("Product.PriceMustBeGreaterThanZero", "Ürün fiyatı sıfırdan büyük olmalıdır.");

        // Stok miktarı negatif olamaz
        public static readonly Error StockCannotBeNegative =
            new("Product.StockCannotBeNegative", "Stok miktarı negatif olamaz.");
    }
}
