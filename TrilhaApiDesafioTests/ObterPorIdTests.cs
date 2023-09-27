using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafioTests.Factories;

namespace TrilhaApiDesafioTests
{
    public class ObterPorIdTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private readonly IntegrationTestAppFactory<Program> _factory;

        public ObterPorIdTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ObterPorId_TarefaExiste_DeveRetornarOkComTarefa()
        {
            // Arrange
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dataAccess = scopedServices.GetRequiredService<OrganizadorContext>();

            var tarefa = TarefaFactory.CriarTarefa();

            dataAccess.Add(tarefa);
            dataAccess.SaveChanges();

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.GetAsync($"/Tarefa/{tarefa.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var tarefaResposta = await response.Content.ReadFromJsonAsync<Tarefa>(jsonOptions);
            tarefaResposta.Should().BeEquivalentTo(tarefa, options => options.ComparingByMembers<Tarefa>());
        }

        [Fact]
        public async Task ObterPorId_TarefaNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            var idInexistente = int.MaxValue;
            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.GetAsync($"/Tarefa/{idInexistente}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}