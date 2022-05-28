using System.Net.Http.Json;
using CloudSnake.Dto;

namespace CloudSnake.Client;

public class GameClient
{
    public async Task<string> CreateGame(string hostPlayerName)
    {
        var request = new CreateGameRequest(hostPlayerName);

        var client = new HttpClient();
        var response = await client.PostAsJsonAsync("https://localhost:7248/games", request);
        var createGameResponse = await response.Content.ReadFromJsonAsync<CreateGameResponse>();

        return createGameResponse.GameCode;
    }

    public async Task<string> JoinGame(string gameCode, string playerName)
    {
        var request = new JoinGameRequest(gameCode, playerName);

        var client = new HttpClient();
        var response = await client.PostAsJsonAsync($"https://localhost:7248/games/{gameCode}/join", request);
        var joinGameResponse = await response.Content.ReadFromJsonAsync<JoinGameResponse>();

        return joinGameResponse.GameCode;
    }



    public async Task PlayerReady(string gameCode, string playerName)
    {
        var request = new JoinGameRequest(gameCode, playerName);

        var client = new HttpClient();
        var response = await client.PostAsJsonAsync($"https://localhost:7248/games/{gameCode}/ready/{playerName}", "null");
        //var joinGameResponse = await response.Content.ReadFromJsonAsync<ReadyPlayerResponse>();
    }
}