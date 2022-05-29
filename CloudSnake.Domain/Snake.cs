using System.Text.Json;

namespace CloudSnake.Domain;

public struct Snake
{
    private static Random _random = new();

    public int Points { get; set; }
    public Orientation Orientation { get; set; }
    public List<Coordinates> Coordinates { get; set; }

    public string ToSnakeData()
    {
        return JsonSerializer.Serialize(this);
    }

    public static Snake FromSnakeData(string snakeData)
    {
        return JsonSerializer.Deserialize<Snake>(snakeData);
    }

    public static Snake RandomSnake(int width, int height, int length)
    {
        var snake = new Snake
        {
            Points = 0,
            Orientation = (Orientation)_random.Next(4),
            Coordinates = new List<Coordinates>()
        };

        var randomX = _random.Next(length, width - length);
        var randomY = _random.Next(length, height - length);

        switch (snake.Orientation)
        {
            case Orientation.North:
                for (int i = 0; i < length; i++)
                {
                    snake.Coordinates.Add(new Coordinates { X = randomX, Y = randomY + i });
                }
                break;
            case Orientation.East:
                for (int i = 0; i < length; i++)
                {
                    snake.Coordinates.Add(new Coordinates { X = randomX - i, Y = randomY });
                }
                break;
            case Orientation.South:
                for (int i = 0; i < length; i++)
                {
                    snake.Coordinates.Add(new Coordinates { X = randomX, Y = randomY - i });
                }
                break;

            case Orientation.West:
                for (int i = 0; i < length; i++)
                {
                    snake.Coordinates.Add(new Coordinates { X = randomX + i, Y = randomY });
                }
                break;
        }

        return snake;
    }
}

public struct Coordinates
{
    public int X { get; set; }
    public int Y { get; set; }
}

public enum Orientation
{
    North,
    East,
    South,
    West
}