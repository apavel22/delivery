using DeliveryApp.Infrastructure;


//using DeliveryApp.Infrastructure.Adapters.Postgres;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Testcontainers.PostgreSql;

using Xunit;

namespace DeliveryApp.IntegrationTests;

public class BaseRepositoryTestsShould: IAsyncLifetime
{
    private ApplicationDbContext _context;

    /// <summary>
    /// ��������� Postgres �� ���������� TestContainers
    /// </summary>
    /// <remarks>�� ���� ��� Docker ��������� � Postgres</remarks>
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:14.7")
        .WithDatabase("good")
        .WithUsername("username")
        .WithPassword("secret")
        .WithCleanUp(true)
        .Build();


    /// <summary>
    /// �������������� ���������
    /// </summary>
    /// <remarks>���������� ����� ������ ������</remarks>
    public async Task InitializeAsync()
    {
        //�������� �� (���������� TestContainers ��������� Docker ��������� � Postgres)
        await _postgreSqlContainer.StartAsync();
        
        //���������� �������� � �����������
        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql(_postgreSqlContainer.GetConnectionString(),
                npgsqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                })
            .Options;

        _context = new ApplicationDbContext(contextOptions);
        _context.Database.Migrate();
    }


    /// <summary>
    /// ���������� ���������
    /// </summary>
    /// <remarks>���������� ����� ������� �����</remarks>
    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync().AsTask();
    }


}