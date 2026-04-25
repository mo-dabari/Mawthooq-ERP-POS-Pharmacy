using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Categories;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Mappers
{
    public static class CategoryMapper
    {
        public static Category ToCategoryDomain(this SqlDataReader sqlData)
        {
            var category = new Category(
              sqlData.GetInt32(sqlData.GetOrdinal("CategoryID")),
              sqlData.GetString(sqlData.GetOrdinal("Name")),
              sqlData.GetString(sqlData.GetOrdinal("Description"))
              );

            if (!sqlData.GetBoolean(sqlData.GetOrdinal("IsActive")))
                category.Deactivate();

                return category;


        }
    }
}
