using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Infrastructure.AdoNet.Shared
{
    internal static class SqlLike
    {
        public static string Escape(string text) =>
            text.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_").Replace("[", "\\[");
    }
}
