using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Mawthooq.Domain.Products;
using Mawthooq.Infrastructure.AdoNet.Persistence.Connections;

namespace Mawthooq.Infrastructure.Services
{
    public class SqlInternalBarcodeSequenceGenerator : IInternalBarcodeSequenceGenerator
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public SqlInternalBarcodeSequenceGenerator(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider
            ?? throw new ArgumentNullException(nameof(connectionProvider));
        }
        public async Task<long> GetNextBarcodeSequenceAsync(int count, CancellationToken cancellationToken = default)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));

            var sequenceName = "dbo.ProductBarcodeSequence";

            string sql = """
                DECLARE @FirstValue SQL_VARIANT;

                EXEC sp_sequence_get_range
                    @sequence_name = @SequenceName,
                    @range_size = @RangeSize,
                    @range_first_value = @FirstValue OUTPUT;

                SELECT CAST(@FirstValue AS BIGINT);
                """;

            SqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

            await using SqlCommand command = new(sql, connection);

            command.Parameters.Add("@SequenceName", SqlDbType.NVarChar, 776).Value = sequenceName;

            command.Parameters.Add("@RangeSize", SqlDbType.BigInt).Value = count;

            var result = await command.ExecuteScalarAsync(cancellationToken);

            if (result is null or DBNull || !long.TryParse(result.ToString(), out long barcode))
                throw new InvalidOperationException($"Failed to retrieve barcode sequence from sequence '{sequenceName}'. Result was null or invalid.");

            return barcode;
        }
    }
}
