using TrilhaApiDesafio.DTO;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafioTests.Helpers
{
    public class TarefaForManipulationDTOBuilder
    {
        private DateTime _data;

        public TarefaForManipulationDTOBuilder()
        {
        }

        public TarefaForManipulationDTOBuilder WithValidDate()
        {
            _data = DateTime.Now;
            return this;
        }

        public TarefaForManipulationDTOBuilder WithInvalidDate()
        {
            _data = DateTime.MinValue;
            return this;
        }

        public TarefaForManipulationDTO Build()
        {
            return new TarefaForManipulationDTO
            {
                Titulo = "Teste",
                Descricao = "Teste de integração",
                Data = _data,
                Status = EnumStatusTarefa.Pendente
            };
        }
    }
}
