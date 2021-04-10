using System.ComponentModel.DataAnnotations;

namespace Lanchat.Tests.Mock.Models
{
    public class ModelWithValidation
    {
        [Required] public string Property { get; set; }
    }
}