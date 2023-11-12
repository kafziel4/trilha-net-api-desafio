using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.DTO;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Services;
using TrilhaApiDesafioTests.Helpers;

namespace TrilhaApiDesafioTests
{
    public class RepositoryTests
    {
        private readonly DbContextOptionsBuilder<OrganizadorContext> _builder;
        private readonly List<Tarefa> _tarefas;

        public RepositoryTests()
        {
            _builder = new DbContextOptionsBuilder<OrganizadorContext>();
            _builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _tarefas = Seed(_builder.Options);
        }

        [Fact]
        public async Task GetTarefaById_IdExistente_DeveRetornarTarefa()
        {
            // Arrange
            using var context = new OrganizadorContext(_builder.Options);
            var repository = new TarefaRepository(context);
            var tarefaEsperada = _tarefas[0];

            // Act
            var result = await repository.GetTarefaById(tarefaEsperada.Id);

            // Assert
            var tarefa = result.Should().BeAssignableTo<TarefaDTO>().Subject;
            tarefa.Id.Should().Be(tarefaEsperada.Id);
            tarefa.Titulo.Should().Be(tarefaEsperada.Titulo);
            tarefa.Descricao.Should().Be(tarefaEsperada.Descricao);
            tarefa.Data.Should().Be(tarefaEsperada.Data);
            tarefa.Status.Should().Be(tarefaEsperada.Status);
        }

        [Fact]
        public async Task GetTarefaById_IdInexistente_DeveRetornarNulo()
        {
            // Arrange
            using var context = new OrganizadorContext(_builder.Options);
            var repository = new TarefaRepository(context);
            var idInexistente = int.MaxValue;

            // Act
            var result = await repository.GetTarefaById(idInexistente);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetTarefas_SemPredicate_DeveRetornarTodasAsTarefas()
        {
            // Arrange
            using var context = new OrganizadorContext(_builder.Options);
            var repository = new TarefaRepository(context);
            var quantidadeTarefas = _tarefas.Count;
            var tarefaEsperada = _tarefas[0];

            // Act
            var tarefas = await repository.GetTarefas();

            // Assert
            tarefas.Should().HaveCount(quantidadeTarefas);
            tarefas[0].Id.Should().Be(tarefaEsperada.Id);
            tarefas[0].Titulo.Should().Be(tarefaEsperada.Titulo);
            tarefas[0].Descricao.Should().Be(tarefaEsperada.Descricao);
            tarefas[0].Data.Should().Be(tarefaEsperada.Data);
            tarefas[0].Status.Should().Be(tarefaEsperada.Status);
        }

        [Fact]
        public async Task GetTarefas_ComPredicate_DeveRetornarTarefasFiltradas()
        {
            // Arrange
            using var context = new OrganizadorContext(_builder.Options);
            var repository = new TarefaRepository(context);
            var tituloEsperado = _tarefas[0].Titulo;

            // Act
            var tarefas = await repository.GetTarefas(x => x.Titulo == tituloEsperado);

            // Assert
            tarefas.TrueForAll(x => x.Titulo == tituloEsperado).Should().BeTrue();
        }

        [Fact]
        public async Task AddTarefa_ComTarefa_DeveAdicionarTarefa()
        {
            // Arrange
            using var context = new OrganizadorContext(_builder.Options);
            var repository = new TarefaRepository(context);
            var tarefaForManipulationDTO = new TarefaForManipulationDTOBuilder()
                .WithValidDate()
                .Build();

            // Act
            var tarefaDTO = await repository.AddTarefa(tarefaForManipulationDTO);

            // Assert
            tarefaDTO.Id.Should().NotBe(0);
            var tarefa = context.Tarefas.First(x => x.Id == tarefaDTO.Id);
            tarefaDTO.Titulo.Should().Be(tarefa.Titulo).And.Be(tarefaForManipulationDTO.Titulo);
            tarefaDTO.Descricao.Should().Be(tarefa.Descricao).And.Be(tarefaForManipulationDTO.Descricao);
            tarefaDTO.Data.Should().Be(tarefa.Data).And.Be(tarefaForManipulationDTO.Data);
            tarefaDTO.Status.Should().Be(tarefa.Status).And.Be(tarefaForManipulationDTO.Status);
        }

        [Fact]
        public async Task UpdateTarefa_ComTarefaExistente_DeveAtualizarTarefa()
        {
            // Arrange
            using var context = new OrganizadorContext(_builder.Options);
            var repository = new TarefaRepository(context);
            var idTarefa = _tarefas[0].Id;
            var tarefaForManipulationDTO = new TarefaForManipulationDTOBuilder()
                .WithValidDate()
                .Build();

            // Act
            var result = await repository.UpdateTarefa(idTarefa, tarefaForManipulationDTO);

            // Assert
            var tarefaDTO = result.Should().BeAssignableTo<TarefaDTO>().Subject;
            tarefaDTO.Id.Should().Be(idTarefa);
            var tarefa = context.Tarefas.First(x => x.Id == tarefaDTO.Id);
            tarefaDTO.Titulo.Should().Be(tarefa.Titulo).And.Be(tarefaForManipulationDTO.Titulo);
            tarefaDTO.Descricao.Should().Be(tarefa.Descricao).And.Be(tarefaForManipulationDTO.Descricao);
            tarefaDTO.Data.Should().Be(tarefa.Data).And.Be(tarefaForManipulationDTO.Data);
            tarefaDTO.Status.Should().Be(tarefa.Status).And.Be(tarefaForManipulationDTO.Status);
        }

        [Fact]
        public async Task UpdateTarefa_ComTarefaInexistente_DeveRetornarNulo()
        {
            // Arrange
            using var context = new OrganizadorContext(_builder.Options);
            var repository = new TarefaRepository(context);
            var idInexistente = int.MaxValue;
            var tarefaForManipulationDTO = new TarefaForManipulationDTOBuilder()
                .WithValidDate()
                .Build();

            // Act
            var result = await repository.UpdateTarefa(idInexistente, tarefaForManipulationDTO);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTarefa_ComTarefaExistente_DeveDeletarTarefa()
        {
            // Arrange
            using var context = new OrganizadorContext(_builder.Options);
            var repository = new TarefaRepository(context);
            var idTarefa = _tarefas[0].Id;

            // Act
            var result = await repository.DeleteTarefa(idTarefa);

            // Assert
            result.Should().BeTrue();
            var tarefa = context.Tarefas.Find(idTarefa);
            tarefa.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTarefa_ComTarefaInexistente_DeveRetornarFalse()
        {
            // Arrange
            using var context = new OrganizadorContext(_builder.Options);
            var repository = new TarefaRepository(context);
            var idInexistente = int.MaxValue;

            // Act
            var result = await repository.DeleteTarefa(idInexistente);

            // Assert
            result.Should().BeFalse();
        }

        private static List<Tarefa> Seed(DbContextOptions<OrganizadorContext> options)
        {
            using var seedContext = new OrganizadorContext(options);
            var tarefas = TarefaFactory.CreateTarefas().ToList();
            seedContext.Tarefas.AddRange(tarefas);
            seedContext.SaveChanges();

            return tarefas;
        }
    }
}
