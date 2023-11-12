using System.Net;
using System.Net.Http.Json;
using TrilhaApiDesafio.DTO;
using TrilhaApiDesafioTests.Helpers;

namespace TrilhaApiDesafioTests
{
    public class CriarTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private const string BasePath = "/Tarefa";
        private readonly IntegrationTestAppFactory<Program> _factory;

        public CriarTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Criar_TarefaValida_DeveRetornarCreatedComTarefa()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var tarefaDTO = new TarefaForManipulationDTOBuilder()
                .WithValidDate()
                .Build();

            // Act
            var response = await httpClient.PostAsJsonAsync($"{BasePath}", tarefaDTO);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseBody = await ResponseHelper.GetResponseBody<TarefaDTO>(response);
            var tarefaResposta = responseBody.Should().BeAssignableTo<TarefaDTO>().Subject;

            tarefaResposta.Id.Should().NotBe(0);
        }

        [Fact]
        public async Task Criar_TarefaComDataVazia_DeveRetornarBadRequest()
        {
            // Arrange
            var tarefaDTO = new TarefaForManipulationDTOBuilder()
                .WithInvalidDate()
                .Build();

            var httpClient = _factory.CreateClient();

            // Act
            var response = await httpClient.PostAsJsonAsync($"{BasePath}", tarefaDTO);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}