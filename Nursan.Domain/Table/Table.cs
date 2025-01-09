namespace Nursan.Domain.Table;

public class Table : Attribute
{
    public string TableName { get; set; }
    public string PrimaryColumn { get; set; }
    public string IdentityColumn { get; set; }

}
