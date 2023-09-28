using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafioTests.Factories;

namespace TrilhaApiDesafioTests
{
    public class DeletarTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private readonly IntegrationTestAppFactory<Program> _factory;

        public DeletarTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Deletar_TarefaExiste_DeveRetornarNoContent()
        {
            // Arrange
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            using var setupScope = _factory.Services.CreateScope();
            var setupScopedServices = setupScope.ServiceProvider;
            var setupDataAccess = setupScopedServices.GetRequiredService<OrganizadorContext>();

            var tarefa = TarefaFactory.CriarTarefa();

            setupDataAccess.Add(tarefa);
            setupDataAccess.SaveChanges();

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.DeleteAsync($"/Tarefa/{tarefa.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dataAccess = scopedServices.GetRequiredService<OrganizadorContext>();
            var tarefaBanco = dataAccess.Tarefas.Find(tarefa.Id);

            tarefaBanco.Should().BeNull();
        }

        [Fact]
        public async Task Deletar_TarefaNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            var idInexistente = int.MaxValue;
            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.DeleteAsync($"/Tarefa/{idInexistente}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}