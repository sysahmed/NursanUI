using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nursan.Domain.AmbarModels;

public partial class TickedRolleNote
{
    [Key]
    public int Id { get; set; }
    
    public string Description { get; set; }
    
    public int? RoleId { get; set; }
    
    public bool? Activ { get; set; }

    [ForeignKey("RoleId")]
    public virtual Roller? Rollers { get; set; }
    
    /// <summary>
    /// Име на ролята - може да се попълва от API отговора (вложен обект Rollers.RoleName или директно като property)
    /// Не е поле в базата данни, използва се само за десериализация от API
    /// </summary>
    [NotMapped]
    [JsonPropertyName("rollerName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RoleName { get; set; }
}

