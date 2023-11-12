using System.Net;
using TrilhaApiDesafio.DTO;
using TrilhaApiDesafioTests.Helpers;

namespace TrilhaApiDesafioTests
{
    public class ObterTests : IClassFixture<IntegrationTestAppFactory<Program>>
    {
        private const string BasePath = "/Tarefa";
        private readonly IntegrationTestAppFactory<Program> _factory;

        public ObterTests(IntegrationTestAppFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ObterPorId_TarefaExiste_DeveRetornarOkComTarefa()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var idTarefa = _factory.Tarefas[0].Id;

            // Act
            var response = await httpClient.GetAsync($"{BasePath}/{idTarefa}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await ResponseHelper.GetResponseBody<TarefaDTO>(response);
            var tarefaResposta = responseBody.Should().BeAssignableTo<TarefaDTO>().Subject;
            tarefaResposta.Id.Should().Be(idTarefa);
        }

        [Fact]
        public async Task ObterPorId_TarefaNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var idInexistente = int.MaxValue;

            // Act
            var response = await httpClient.GetAsync($"{BasePath}/{idInexistente}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ObterTodos_TarefasExistem_DeveRetornarOkComTarefas()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var quantidadeTarefas = _factory.Tarefas.Count;

            // Act
            var response = await httpClient.GetAsync($"{BasePath}/ObterTodos");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var tarefasResposta = await ResponseHelper.GetResponseBody<IEnumerable<TarefaDTO>>(response);
            tarefasResposta.Should().HaveCount(quantidadeTarefas);
        }

        [Fact]
        public async Task ObterPorTitulo_TarefasExistem_DeveRetornarOkComTarefas()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var tituloTarefa = _factory.Tarefas[0].Titulo;

            // Act
            var response = await httpClient.GetAsync($"{BasePath}/ObterPorTitulo?titulo={tituloTarefa}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await ResponseHelper.GetResponseBody<IEnumerable<TarefaDTO>>(response);
            var tarefasResposta = responseBody.Should().BeAssignableTo<IEnumerable<TarefaDTO>>().Subject;
            tarefasResposta.All(x => x.Titulo == tituloTarefa).Should().BeTrue();
        }

        [Fact]
        public async Task ObterPorData_TarefasExistem_DeveRetornarOkComTarefas()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var dataTarefa = _factory.Tarefas[0].Data;

            // Act
            var response = await httpClient.GetAsync($"{BasePath}/ObterPorData?data={dataTarefa:yyyy-MM-dd}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await ResponseHelper.GetResponseBody<IEnumerable<TarefaDTO>>(response);
            var tarefasResposta = responseBody.Should().BeAssignableTo<IEnumerable<TarefaDTO>>().Subject;
            tarefasResposta.All(x => x.Data == dataTarefa).Should().BeTrue();
        }

        [Fact]
        public async Task ObterPorStatus_TarefasExistem_DeveRetornarOkComTarefas()
        {
            // Arrange
            var httpClient = _factory.CreateClient();
            var statusTarefa = _factory.Tarefas[0].Status;

            // Act
            var response = await httpClient.GetAsync($"{BasePath}/ObterPorStatus?status={statusTarefa}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseBody = await ResponseHelper.GetResponseBody<IEnumerable<TarefaDTO>>(response);
            var tarefasResposta = responseBody.Should().BeAssignableTo<IEnumerable<TarefaDTO>>().Subject;
            tarefasResposta.All(x => x.Status == statusTarefa).Should().BeTrue();
        }
    }
}