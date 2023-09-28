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
                    Status = IsEven(i) ? EnumStatusTarefa.Pendente : EnumStatusTarefa.Finalizado
                });
            }

            return tarefas;
        }

        public static bool IsEven(int n) => n % 2 == 0;
    }
}
