using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafioTests.Factories
{
    public static class TarefaFactory
    {
        public static Tarefa CriarTarefa() =>
            new()
            {
                Titulo = "Teste",
                Descricao = "Teste de integração",
                Data = DateTime.Now,
                Status = EnumStatusTarefa.Pendente
            };
    }
}
