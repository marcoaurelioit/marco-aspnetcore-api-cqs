using Dapper;
using Marco.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace Marco.AspNetCore.Cqs.Infra.Data.Dapper.Context
{
    public class ContextReadOnly
    {
        private readonly SqlServerReadOnlySettings sqlServerReadOnlySettings;
        private IDbConnection Connection => new SqlConnection(sqlServerReadOnlySettings.DefaultConnection);

        public ContextReadOnly(SqlServerReadOnlySettings sqlServerReadOnlySettings)
        {
            this.sqlServerReadOnlySettings = sqlServerReadOnlySettings ?? throw new ArgumentNullException(nameof(sqlServerReadOnlySettings));
        }       

        #region [+] Query
        public virtual IEnumerable<T> Query<T>(string sql, object param = null) where T : Entity =>
            Connection.Query<T>(sql, param);

        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null) where T : Entity =>
            await Connection.QueryAsync<T>(sql, param);
        #endregion
    }
}