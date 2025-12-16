using System.Data;
using Microsoft.Data.Sqlite;

namespace TheDependencyProblem.Data;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateDbConnectionAsync();
}

public class MySqlDbConnectionFactory : IDbConnectionFactory
{
    private readonly DbConnectionOptions _connectionOptions = new()
    {
        ConnectionString = "Server=localhost;Database=mydb;User=myuser;Password=mypassword;"
    };

    public async Task<IDbConnection> CreateDbConnectionAsync()
    {
        var connection = new MySql.Data.MySqlClient.MySqlConnection(_connectionOptions.ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}

public class SqliteDbConnectionFactory : IDbConnectionFactory
{
    private readonly DbConnectionOptions _connectionOptions = new()
    {
        ConnectionString = "Data Source=./database.db"
    };

    public async Task<IDbConnection> CreateDbConnectionAsync()
    {
        var connection = new SqliteConnection(_connectionOptions.ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}
