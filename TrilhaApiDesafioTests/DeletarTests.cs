using System.Net;
using TrilhaApiDesafioTests.Helpers;

namespace TrilhaApiDesafioTests
{
    public class DeletarTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private const string BasePath = "/Tarefa";
        private readonly IntegrationTestAppFactory<Program> _factory;

        public DeletarTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Deletar_TarefaExiste_DeveRetornarNoContent()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var idTarefa = _factory.Tarefas[0].Id;

            // Act
            var response = await httpClient.DeleteAsync($"{BasePath}/{idTarefa}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Deletar_TarefaNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var idInexistente = int.MaxValue;

            // Act
            var response = await httpClient.DeleteAsync($"{BasePath}/{idInexistente}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}