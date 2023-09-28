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
    public class AtualizarTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private readonly IntegrationTestAppFactory<Program> _factory;

        public AtualizarTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Atualizar_TarefaValida_DeveRetornarOkComTarefa()
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

            var tarefas = TarefaFactory.CriarTarefas(quantidade: 2);
            var setupTarefaBanco = tarefas[0];
            var tarefaRequisicao = tarefas[1];

            setupDataAccess.Add(setupTarefaBanco);
            setupDataAccess.SaveChanges();

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.PutAsJsonAsync($"/Tarefa/{setupTarefaBanco.Id}", tarefaRequisicao);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await response.Content.ReadFromJsonAsync<Tarefa>(jsonOptions);
            var tarefaResposta = responseBody.Should().BeAssignableTo<Tarefa>().Subject;

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dataAccess = scopedServices.GetRequiredService<OrganizadorContext>();
            var tarefaBanco = dataAccess.Tarefas.Find(tarefaResposta.Id);

            tarefaResposta.Should().BeEquivalentTo(tarefaBanco, options => options.ComparingByMembers<Tarefa>());
        }

        [Fact]
        public async Task Atualizar_TarefaNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            var idInexistente = int.MaxValue;
            var tarefa = TarefaFactory.CriarTarefa();

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.PutAsJsonAsync($"/Tarefa/{idInexistente}", tarefa);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Atualizar_TarefaComDataVazia_DeveRetornarBadRequest()
        {
            // Arrange   
            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var dataAccess = scopedServices.GetRequiredService<OrganizadorContext>();

            var tarefas = TarefaFactory.CriarTarefas(quantidade: 2);
            var tarefaBanco = tarefas[0];
            var tarefaRequisicao = tarefas[1];

            dataAccess.Add(tarefaBanco);
            dataAccess.SaveChanges();

            tarefaRequisicao.Data = DateTime.MinValue;

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.PutAsJsonAsync($"/Tarefa/{tarefaBanco.Id}", tarefaRequisicao);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}