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
    public class ObterPorTituloTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private readonly IntegrationTestAppFactory<Program> _factory;

        public ObterPorTituloTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ObterPorTitulo_TarefasExistem_DeveRetornarOkComTarefa()
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

            var tarefas = TarefaFactory.CriarTarefas(quantidade: 3);
            var tarefaEsperada = tarefas[0];

            dataAccess.AddRange(tarefas);
            dataAccess.SaveChanges();

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.GetAsync($"/Tarefa/ObterPorTitulo?titulo={tarefaEsperada.Titulo}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await response.Content.ReadFromJsonAsync<IEnumerable<Tarefa>>(jsonOptions);
            var tarefasResposta = responseBody.Should().BeAssignableTo<IEnumerable<Tarefa>>().Subject;
            tarefasResposta.Should().HaveCount(1);
            tarefasResposta.First().Should().BeEquivalentTo(tarefaEsperada, options => options.ComparingByMembers<Tarefa>());
        }
    }
}