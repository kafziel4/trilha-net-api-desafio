using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MsSql;
using TrilhaApiDesafio.Context;

namespace TrilhaApiDesafioTests.Factories
{
    public class IntegrationTestAppFactory<TStartup>
        : WebApplicationFactory<TStartup>, IAsyncLifetime where TStartup : class
    {
        private readonly MsSqlContainer _msSqlcontainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2019-CU18-ubuntu-20.04")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var connectionString = _msSqlcontainer.GetConnectionString();

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<OrganizadorContext>));
                services.AddDbContext<OrganizadorContext>(options => options.UseSqlServer(connectionString));

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<OrganizadorContext>();
                context.Database.EnsureCreated();
            });
        }

        public async Task InitializeAsync()
        {
            await _msSqlcontainer.StartAsync();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _msSqlcontainer.DisposeAsync();
        }
    }
}
