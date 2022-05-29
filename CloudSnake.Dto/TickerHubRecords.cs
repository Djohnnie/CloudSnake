using CloudSnake.Domain;

namespace CloudSnake.Dto;

public record GameState(int Width, int Height, bool IsReady, string GameCode, List<PlayerState> Players, Food Food);
public record PlayerState(string PlayerName, bool IsReady, Snake Snake);