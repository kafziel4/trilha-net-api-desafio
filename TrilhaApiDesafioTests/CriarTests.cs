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
    public class CriarTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private readonly IntegrationTestAppFactory<Program> _factory;

        public CriarTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Criar_TarefaValida_DeveRetornarCreatedComTarefa()
        {
            // Arrange
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.PostAsJsonAsync("/Tarefa", TarefaFactory.CriarTarefa());

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseBody = await response.Content.ReadFromJsonAsync<Tarefa>(jsonOptions);
            var tarefaResposta = responseBody.Should().BeAssignableTo<Tarefa>().Subject;

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dataAccess = scopedServices.GetRequiredService<OrganizadorContext>();
            var tarefaBanco = dataAccess.Tarefas.Find(tarefaResposta.Id);

            tarefaResposta.Should().BeEquivalentTo(tarefaBanco, options => options.ComparingByMembers<Tarefa>());
        }

        [Fact]
        public async Task Criar_TarefaComDataVazia_DeveRetornarBadRequest()
        {
            // Arrange
            var tarefa = TarefaFactory.CriarTarefa();
            tarefa.Data = DateTime.MinValue;

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.PostAsJsonAsync("/Tarefa", tarefa);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}