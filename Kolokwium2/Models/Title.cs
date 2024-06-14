namespace Kolokwium2.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("titles")]
public class Title
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    private ICollection<CharacterTitle> CharacterTitles { get; set; } = new HashSet<CharacterTitle>();
}