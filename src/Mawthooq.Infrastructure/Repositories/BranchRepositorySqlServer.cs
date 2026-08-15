using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Branches;
using Mawthooq.Domain.Common.Interfaces;
using Mawthooq.Domain.ValueObjects;
using Mawthooq.Infrastructure.Data;
using Mawthooq.Infrastructure.Data.Helpers;
using Mawthooq.Infrastructure.Mappers;
using Microsoft.Data.SqlClient;

namespace Mawthooq.Infrastructure.Repositories
{
    public class BranchRepositorySqlServer : IBranchRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public BranchRepositorySqlServer(IDbConnectionFactory connectionFactory)
        {
            ArgumentNullException.ThrowIfNull(connectionFactory, nameof(connectionFactory));
            _connectionFactory = connectionFactory;
        }

        // public Branch Add(Branch branch, Address address)
        // {
        //     ArgumentNullException.ThrowIfNull(branch, nameof(branch));
        //     ArgumentNullException.ThrowIfNull(address, nameof(address));

        //     using SqlConnection connection = (SqlConnection)_connectionFactory.CreateConnection();

        //     string query = @"Insert
        //                     Into
        //                         Branch
        //                             (Name, AddressId)
        //                     Values
        //                         (@Name, @AddressId)
        //                         Select Cast(SCOPE_IDENTITY() as Int)";
        //     SqlTransaction? transaction = null;
        //     try
        //     {
        //         connection.Open();
        //         transaction = connection.BeginTransaction();


        //         int newAddressId = AddressHelper.InsertAddress(address, connection, transaction);

        //         if (newAddressId != 0)
        //         {
        //             using SqlCommand command = new(query, connection, transaction);

        //             command.Parameters.AddWithValue("@Name", branch.Name).DbType = DbType.String;
        //             command.Parameters.AddWithValue("@AddressId", newAddressId).DbType = DbType.Int32;

        //             object result = command.ExecuteScalar();
        //             if (int.TryParse(result.ToString(), out int newBranchId))
        //             {
        //                 transaction.Commit();
        //                 return new Branch(newBranchId, branch.Name, newAddressId);
        //             }
        //             throw new InvalidOperationException("Failed to insert Branch.");
        //         }
        //         throw new InvalidOperationException("Failed to insert Address.");
        //     }
        //     catch
        //     {
        //         transaction?.Rollback();
        //         throw;
        //     }
        // }

        public Branch? Add(Branch entity)
        {
            throw new NotImplementedException();
        }

        public Branch Add(Branch branch, Address address)
        {
            throw new NotImplementedException();
        }

        public void Delete(int branchId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(branchId, nameof(branchId));

            using var connection = _connectionFactory.CreateConnection();

            string query = @"Delete
                            From
                                Branch
                            Where
                                BranchId = @BranchId";

            using SqlCommand command = new(query, (SqlConnection)connection);
            command.Parameters.AddWithValue("@BranchId", branchId).DbType = DbType.Int32;

            connection.Open();
            command.ExecuteNonQuery();
        }

        public Branch? GetById(int branchId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(branchId, nameof(branchId));

            using var connection = _connectionFactory.CreateConnection();

            string query = @"Select
                                b.BranchId,
                                b.Name,
                                b.AddressId,
                                a.BuildingNumber,
                                a.Street,
                                Ci.Name as CityName,
                                G.Name as GovernorateName,
                                Co.Name as CountryName,
                                b.IsActive
                            From
                                Branch as b
                            Join
                                Address as a
                            on
                                b.AddressId = a.AddressId
                            Join
                                City as Ci
                            on
                                a.City = Ci.CityId
                            Join
                                Governorate as G
                            on
                                A.Governorate = G.GovernorateId
                            Join
                                Country as Co
                            on
                                A.Country = Co.CountryId
                            Where
                                b.BranchId = @BranchId";

            using SqlCommand command = new(query, (SqlConnection)connection);
            command.Parameters.AddWithValue("@BranchId", branchId).DbType = DbType.Int32;

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
                return reader.ToBranchDomain();

            return null;
        }

        public void Update(Branch entity)
        {
            throw new NotImplementedException();
        }

        // public void Update(Branch entity)
        // {
        //     ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        //     using var connection = (SqlConnection)_connectionFactory.CreateConnection();

        //     string query = @"Update
        //                         Branch
        //                     Set
        //                         Name = @Name,
        //                         IsActive = @IsActive
        //                     Where
        //                         BranchId = @BranchId";

        //     connection.Open();
        //     using SqlTransaction transaction = connection.BeginTransaction();

        //     try
        //     {
        //         AddressHelper.UpdateAddress(entity.AddressId, entity.Address, connection, transaction);

        //         using SqlCommand command = new(query, connection, transaction);

        //         command.Parameters.AddWithValue("@Name", entity.Name).DbType = DbType.String;
        //         command.Parameters.AddWithValue("@IsActive", entity.IsActive).DbType = DbType.Boolean;
        //         command.Parameters.AddWithValue("@BranchId", entity.BranchID).DbType = DbType.Int32;

        //         command.ExecuteNonQuery();

        //         transaction.Commit();
        //     }
        //     catch
        //     {
        //         transaction.Rollback();
        //         throw;
        //     }
        // }

    }
}
