using System.Threading.Tasks;
using Npgsql;

namespace NpgsqlDbExtensions
{
    public static class NpgsqlDataReaderExtension
    {
        public static async Task<string?> ReadNullOrStringAsync(this NpgsqlDataReader reader, int ordinal)
        {
            return await reader.IsDBNullAsync(ordinal) ? null : reader.GetString(ordinal);
        }

        public static string? ReadNullOrString(this NpgsqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }
    }
}