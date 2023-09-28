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
    public class ObterPorStatusTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private readonly IntegrationTestAppFactory<Program> _factory;

        public ObterPorStatusTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ObterPorStatus_TarefasExistem_DeveRetornarOkComTarefas()
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

            var tarefas = TarefaFactory.CriarTarefas(quantidade: 5);
            var statusEsperado = EnumStatusTarefa.Pendente;
            var tarefasEsperadas = tarefas.Where(x => x.Status == statusEsperado).ToList();

            dataAccess.AddRange(tarefas);
            dataAccess.SaveChanges();

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.GetAsync($"/Tarefa/ObterPorStatus?status={statusEsperado}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await response.Content.ReadFromJsonAsync<IEnumerable<Tarefa>>(jsonOptions);
            var tarefasResposta = responseBody.Should().BeAssignableTo<IEnumerable<Tarefa>>().Subject;
            tarefasResposta.Should().BeEquivalentTo(tarefasEsperadas, options => options.ComparingByMembers<Tarefa>());
        }
    }
}