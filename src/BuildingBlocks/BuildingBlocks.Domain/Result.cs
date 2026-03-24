using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain
{
    // Bir işlemin başarılı veya hatalı sonucunu temsil eder
    public class Result
    {
        // İşlemin başarılı olup olmadığını gösterir
        public bool IsSuccess { get; }

        // İşlemin başarısız olup olmadığını gösterir
        public bool IsFailure => !IsSuccess;

        // Başarısız durumda hata bilgisini taşır
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException(
                    "Başarılı sonuç hata içeremez. / A successful result cannot contain an error.");
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException(
                    "Başarısız sonuç bir hata içermelidir. / A failed result must contain an error.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        // Başarılı sonuç üretir
        public static Result Success() => new(true, Error.None);

        // Başarısız sonuç üretir
        public static Result Failure(Error error) => new(false, error);
    }

    // Veri dönen işlemler için generic sonuç sınıfı
    public class Result<T> : Result
    {
        // Başarılı işlem sonucunda dönen veri
        public T Value { get; }

        protected Result(T value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            Value = value;
        }

        // Veri içeren başarılı sonuç üretir
        public static Result<T> Success(T value) => new(value, true, Error.None);

        // Veri içermeyen başarısız sonuç üretir
        public static new Result<T> Failure(Error error) => new(default!, false, error);
    }
}
