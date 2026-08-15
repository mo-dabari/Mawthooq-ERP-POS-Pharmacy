using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Branches;
using Mawthooq.Domain.ValueObjects;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Mappers
{
    public static class BranchMapper
    {
        public static Branch ToBranchDomain(this SqlDataReader sqlData)
        {
            Address branchAddress = new Address(
                sqlData.GetInt32(sqlData.GetOrdinal("BuildingNumber")),
                sqlData.GetString(sqlData.GetOrdinal("Street")),
                sqlData.GetString(sqlData.GetOrdinal("City")),
                sqlData.GetString(sqlData.GetOrdinal("State")),
                sqlData.GetString(sqlData.GetOrdinal("Country"))
            );
            var branch = new Branch(
              sqlData.GetInt32(sqlData.GetOrdinal("BranchId")),
              sqlData.GetString(sqlData.GetOrdinal("Name")),
                branchAddress);

            if (!sqlData.GetBoolean(sqlData.GetOrdinal("IsActive")))
                branch.Deactivate();

            return branch;
        }
    }
}

