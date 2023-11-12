using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafioTests.Helpers
{
    public static class TarefaFactory
    {
        public static IEnumerable<Tarefa> CreateTarefas(int quantidade = 5)
        {
            for (int i = 1; i <= quantidade; i++)
            {
                yield return new Tarefa
                {
                    Titulo = $"Testes - {i}",
                    Descricao = "Testes de integração",
                    Data = DateTime.Now.AddDays(i),
                    Status = IsEven(i) ? EnumStatusTarefa.Pendente : EnumStatusTarefa.Finalizado
                };
            }
        }

        public static bool IsEven(int n) => n % 2 == 0;
    }
}
