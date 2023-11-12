using System.ComponentModel.DataAnnotations;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.DTO
{
    public class TarefaForManipulationDTO : IValidatableObject
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public EnumStatusTarefa Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Data == DateTime.MinValue)
            {
                yield return new ValidationResult(
                    "A data da tarefa não pode ser vazia.", new[] { nameof(Data) });
            }
        }
    }
}
