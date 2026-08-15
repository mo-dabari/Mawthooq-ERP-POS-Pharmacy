using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Mawthooq.Domain.Common.Interfaces;
using Microsoft.Data.SqlClient;
using Mawthooq.Domain.Products;
using Mawthooq.Infrastructure.ADO.Persistence.Connections;

namespace Mawthooq.Infrastructure.Services
{
    public class SqlInternalBarcodeSequenceGenerator : IInternalBarcodeSequenceGenerator
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public SqlInternalBarcodeSequenceGenerator(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory
            ?? throw new ArgumentNullException(nameof(sqlConnectionFactory));
        }
        public long GetNextBarcodeSequence(int count)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count, nameof(count));

            // // سنستدعي الـ Sequence باستخدام SQL عادي لجلب الرقم الحالي وتحريك العداد بالـ count المطلوب
            // // نستخدم sp_sequence_get_range وهي ميزة في SQL Server لحجز نطاق أرقام دفعة واحدة بكفاءة عالية
            // var results = new List<long>();

            // تجهيز البارامترز لـ الميزة الرسمية في SQL Server لحجز نطاق
            var sequenceName = "dbo.ProductBarcodeSequence";

            // كود افتراضي مبسط لجلب النطاق عبر الاستعلام:
            // بدلاً من التعقيد، نتحرك بجملة تكرارية سريعة أو عبر الـ Procedure الرسمي:
            using var command = (SqlConnection)_sqlConnectionFactory.Database.GetDbConnection().CreateCommand();
            _sqlConnectionFactory.command.CommandText = $@"
            DECLARE @FirstValue SQL_VARIANT;
            EXEC sp_sequence_get_range
                @sequence_name = N'{sequenceName}',
                @range_size = {count},
                @range_first_value = @FirstValue OUTPUT;
            SELECT CAST(@FirstValue AS BIGINT);";

            if (command.Connection.State != ConnectionState.Open)
                command.Connection.OpenAsync();

            var firstValue = (long)command.ExecuteScalar();

            // // بناء قائمة الأرقام المحجوزة بناءً على أول رقم أعطته لنا الداتابيز
            // for (int i = 0; i < count; i++)
            // {
            //     results.Add(firstValue + i);
            // }

            // return results;

            return firstValue;
        }
    }
}
