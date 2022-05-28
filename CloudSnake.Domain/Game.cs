namespace CloudSnake.Domain;

public class Game
{
    public Guid Id { get; set; }
    public int SysId { get; set; }
    public string Code { get; set; }
    public bool IsActive { get; set; }

    public virtual ICollection<Player> Players { get; set; }
}