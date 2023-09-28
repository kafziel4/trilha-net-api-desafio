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

        public static List<Tarefa> CriarTarefas(int quantidade)
        {
            List<Tarefa> tarefas = new();

            for (int i = 1; i <= quantidade; i++)
            {
                tarefas.Add(new Tarefa
                {
                    Titulo = $"Testes - {i}",
                    Descricao = "Testes de integração",
                    Data = DateTime.Now.AddDays(i),
                    Status = EnumStatusTarefa.Pendente
                });
            }

            return tarefas;
        }
    }
}
