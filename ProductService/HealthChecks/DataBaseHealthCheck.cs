using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SayyehBanTools.ConnectionDB;
using System.Data.Common;

namespace ProductService.HealthChecks
{
    public class DataBaseHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //
            using (var connection = new SqlConnection(SqlServerConnection.ConnectionString("4Mgp5catJqMnBNvqAqdJ2w==", "eWyP6NKkWfiTzk6B1pz8gw==", "m6UQxl628s/a1Hx1CxA2LQ==", "xbfQyKCUrBvw5zxn8sMOfg==", "257ld6s4dsc16e2j", "69q18j991xl48u6u")))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);
                    var command = connection.CreateCommand();
                    command.CommandText = "select 1";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                }
                catch (DbException ex)
                {
                    return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
                }
            }
            return HealthCheckResult.Healthy();

        }
    }
}
