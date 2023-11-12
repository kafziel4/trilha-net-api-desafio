using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.DTO;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Services
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly OrganizadorContext _context;

        public TarefaRepository(OrganizadorContext context)
        {
            _context = context;
        }

        public async Task<TarefaDTO?> GetTarefaById(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);

            return tarefa == null ? null : TarefaToDTO(tarefa);
        }

        public async Task<List<TarefaDTO>> GetTarefas(Expression<Func<Tarefa, bool>>? predicate = null)
        {
            var collection = _context.Tarefas as IQueryable<Tarefa>;

            if (predicate != null)
            {
                collection = collection.Where(predicate);
            }

            var tarefas = await collection
                .Select(t => new TarefaDTO
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Descricao = t.Descricao,
                    Data = t.Data,
                    Status = t.Status,
                })
                .ToListAsync();

            return tarefas;
        }

        public async Task<TarefaDTO> AddTarefa(TarefaForManipulationDTO tarefaDTO)
        {
            var tarefa = TarefaFromDTO(tarefaDTO);
            _context.Add(tarefa);
            await _context.SaveChangesAsync();

            return TarefaToDTO(tarefa);
        }

        public async Task<TarefaDTO?> UpdateTarefa(int id, TarefaForManipulationDTO tarefaDTO)
        {
            var exists = _context.Tarefas.Any(t => t.Id == id);
            if (!exists)
            {
                return null;
            }

            var tarefa = TarefaFromDTO(tarefaDTO, id);
            _context.Entry(tarefa).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return TarefaToDTO(tarefa);
        }

        public async Task<bool> DeleteTarefa(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null)
            {
                return false;
            }

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();

            return true;
        }

        private static TarefaDTO TarefaToDTO(Tarefa tarefa)
        {
            return new TarefaDTO
            {
                Id = tarefa.Id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Data = tarefa.Data,
                Status = tarefa.Status
            };
        }

        private static Tarefa TarefaFromDTO(TarefaForManipulationDTO tarefa, int id = 0)
        {
            return new Tarefa
            {
                Id = id,
                Titulo = tarefa.Titulo,
                Descricao = tarefa.Descricao,
                Data = tarefa.Data,
                Status = tarefa.Status
            };
        }
    }
}
