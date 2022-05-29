using System.Text.Json;

namespace CloudSnake.Domain;

public struct Food
{
    public List<Bite> Bites { get; set; }

    public string ToFoodData()
    {
        return JsonSerializer.Serialize(this);
    }

    public static Food FromFoodData(string foodData)
    {
        return JsonSerializer.Deserialize<Food>(foodData);
    }
}

public struct Bite
{
    public int X { get; set; }
    public int Y { get; set; }
}