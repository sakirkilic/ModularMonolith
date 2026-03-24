using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain
{
    // İş kuralı veya uygulama hatasını temsil eder
    public sealed record Error
    {
        // Hatasız durumu temsil eden sabit değer
        public static readonly Error None = new(string.Empty, string.Empty);

        // Hatanın benzersiz kodu
        public string Code { get; }

        // Hatanın kullanıcı veya geliştirici için açıklaması
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
