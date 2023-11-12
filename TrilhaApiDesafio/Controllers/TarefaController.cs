using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.DTO;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Services;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaController(ITarefaRepository tarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var tarefa = await _tarefaRepository.GetTarefaById(id);

            return tarefa == null ? NotFound() : Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
        {
            var tarefas = await _tarefaRepository.GetTarefas();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            var tarefas = await _tarefaRepository.GetTarefas(x => x.Titulo.Contains(titulo));

            return Ok(tarefas);
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            var tarefas = await _tarefaRepository.GetTarefas(x => x.Data.Date == data.Date);

            return Ok(tarefas);
        }

        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefas = await _tarefaRepository.GetTarefas(x => x.Status == status);

            return Ok(tarefas);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(TarefaForManipulationDTO tarefaDTO)
        {
            var novaTarefa = await _tarefaRepository.AddTarefa(tarefaDTO);

            return CreatedAtAction(nameof(ObterPorId), new { id = novaTarefa.Id }, novaTarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, TarefaForManipulationDTO tarefaDTO)
        {
            var tarefaAtualizada = await _tarefaRepository.UpdateTarefa(id, tarefaDTO);

            return tarefaAtualizada == null ? NotFound() : Ok(tarefaAtualizada);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var tarefaDeletada = await _tarefaRepository.DeleteTarefa(id);

            return tarefaDeletada ? NoContent() : NotFound();
        }
    }
}
