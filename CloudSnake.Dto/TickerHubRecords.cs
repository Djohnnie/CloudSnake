namespace CloudSnake.Dto;

public record GameState(string GameCode, List<PlayerState> Players);
public record PlayerState(string PlayerName, bool IsReady);