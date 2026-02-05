using System.Globalization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dometrain.EFCore.API.Data.ValueConverters
{
    public class DateTimeToChar8Converter() : ValueConverter<DateTime, string>(
        v => v.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
        v => DateTime.ParseExact(v, "yyyyMMdd", CultureInfo.InvariantCulture))
    {
        // Convert DateTime to string (char(8)) for storage
        // Convert string (char(8)) back to DateTime
    }
}
