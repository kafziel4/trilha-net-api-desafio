using System.Linq.Expressions;
using TrilhaApiDesafio.DTO;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Services
{
    public interface ITarefaRepository
    {
        Task<TarefaDTO?> GetTarefaById(int id);
        Task<List<TarefaDTO>> GetTarefas(Expression<Func<Tarefa, bool>>? predicate = null);
        Task<TarefaDTO> AddTarefa(TarefaForManipulationDTO tarefaDTO);
        Task<TarefaDTO?> UpdateTarefa(int id, TarefaForManipulationDTO tarefaDTO);
        Task<bool> DeleteTarefa(int id);
    }
}