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
    public class ObterTodosTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private readonly IntegrationTestAppFactory<Program> _factory;

        public ObterTodosTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ObterTodos_TarefasExistem_DeveRetornarOkComTarefas()
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

            var quantidadeTarefas = 3;
            var tarefas = TarefaFactory.CriarTarefas(quantidade: quantidadeTarefas);

            dataAccess.AddRange(tarefas);
            dataAccess.SaveChanges();

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.GetAsync("/Tarefa/ObterTodos");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var tarefasResposta = await response.Content.ReadFromJsonAsync<IEnumerable<Tarefa>>(jsonOptions);
            tarefasResposta.Should().BeEquivalentTo(tarefas, options => options.ComparingByMembers<Tarefa>());
        }
    }
}