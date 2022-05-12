namespace CloudSnake.Domain;

public class Player
{
    public Guid Id { get; set; }
    public int SysId { get; set; }
    public string Name { get; set; }

    public Game Game { get; set; }
}