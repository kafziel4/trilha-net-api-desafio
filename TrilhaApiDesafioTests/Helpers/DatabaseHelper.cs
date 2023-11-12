using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafioTests.Helpers
{
    public static class DatabaseHelper
    {
        public static List<Tarefa> Seed(OrganizadorContext context)
        {
            var tarefas = TarefaFactory.CreateTarefas().ToList();
            context.AddRange(tarefas);
            context.SaveChanges();

            return tarefas;
        }
    }
}
