using System.Net;
using System.Net.Http.Json;
using TrilhaApiDesafio.DTO;
using TrilhaApiDesafioTests.Helpers;

namespace TrilhaApiDesafioTests
{
    public class AtualizarTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private const string BasePath = "/Tarefa";
        private readonly IntegrationTestAppFactory<Program> _factory;

        public AtualizarTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Atualizar_TarefaValida_DeveRetornarOkComTarefa()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var idTarefa = _factory.Tarefas[0].Id;
            var tarefaDTO = new TarefaForManipulationDTOBuilder()
                .WithValidDate()
                .Build();

            // Act
            var response = await httpClient.PutAsJsonAsync($"{BasePath}/{idTarefa}", tarefaDTO);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await ResponseHelper.GetResponseBody<TarefaDTO>(response);
            var tarefaResposta = responseBody.Should().BeAssignableTo<TarefaDTO>().Subject;
            tarefaResposta.Id.Should().Be(idTarefa);
        }

        [Fact]
        public async Task Atualizar_TarefaNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var idInexistente = int.MaxValue;
            var tarefaDTO = new TarefaForManipulationDTOBuilder()
                .WithValidDate()
                .Build();


            // Act
            var response = await httpClient.PutAsJsonAsync($"{BasePath}/{idInexistente}", tarefaDTO);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Atualizar_TarefaComDataVazia_DeveRetornarBadRequest()
        {
            // Arrange   
            var httpClient = _factory.CreateClient();
            var idTarefa = _factory.Tarefas[0].Id;
            var tarefaDTO = new TarefaForManipulationDTOBuilder()
                .WithInvalidDate()
                .Build();

            // Act
            var response = await httpClient.PutAsJsonAsync($"{BasePath}/{idTarefa}", tarefaDTO);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}