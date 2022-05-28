namespace CloudSnake.Dto;

public record CreateGameRequest(string HostPlayerName);
public record CreateGameResponse(string GameCode);

public record JoinGameRequest(string GameCode, string PlayerName);
public record JoinGameResponse(string GameCode);

public record ReadyPlayerRequest(string GameCode, string PlayerName);
public record ReadyPlayerResponse();

public record GetActiveGamesResponse(List<ActiveGame> ActiveGames);
public record ActiveGame(string GameCode, List<ActivePlayer> Players);
public record ActivePlayer(string PlayerName, bool IsReady);