using CloudSnake.Client;
using CloudSnake.Dto;
using CloudSnake.WinForms.Enums;
using Microsoft.AspNetCore.SignalR.Client;

namespace CloudSnake.WinForms;

public partial class MainForm : Form
{
    private readonly Random _random = new();
    private readonly GameClient _gameClient;
    private readonly string _clientId = $"{Guid.NewGuid()}";
    private bool _toggle = false;

    private SnakeGameState _gameState = SnakeGameState.MainMenu;
    public SnakeGameState GameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            RefreshGameState();
        }
    }

    public MainForm(GameClient gameClient)
    {
        _gameClient = gameClient;

        InitializeComponent();
    }

    private HubConnection _connection;

    private async void MainForm_Load(object sender, EventArgs e)
    {
        GameState = SnakeGameState.MainMenu;

        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7080/ticker")
            .Build();

        _connection.Closed += async (error) =>
        {
            await Task.Delay(_random.Next(0, 5) * 1000);
            await _connection.StartAsync();
        };

        _connection.On<GameState>("ReceiveGameState", (gameState) =>
        {
            Invoke(() =>
            {
                playersListBox.Items.Clear();
                currentGameCodeLabel.Text = gameState.GameCode;

                foreach (var player in gameState.Players)
                {
                    playersListBox.Items.Add($"{player.PlayerName} ({(player.IsReady ? "Ready" : "Not Ready")})");
                }
            });
        });

        try
        {
            while (_connection.State != HubConnectionState.Connected)
            {
                await Task.Delay(1000);

                try
                {
                    await _connection.StartAsync();
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            Text = ex.Message;
        }
    }

    private async void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (_connection.State == HubConnectionState.Connected)
        {
            await _connection.SendAsync("Turn", _clientId);
        }
    }

    private void newGameButton_Click(object sender, EventArgs e)
    {
        GameState = SnakeGameState.NewGame;
    }

    private void joinGameButton_Click(object sender, EventArgs e)
    {
        GameState = SnakeGameState.JoinGame;
    }

    private async void lobbyButton_Click(object sender, EventArgs e)
    {
        var gameCode = string.Empty;

        if (GameState == SnakeGameState.NewGame)
        {
            gameCode = await _gameClient.CreateGame(playerNameTextBox.Text);
        }

        if (GameState == SnakeGameState.JoinGame)
        {
            gameCode = await _gameClient.JoinGame(gameCodeTextBox.Text, playerNameTextBox.Text);
        }

        await _connection.SendAsync("JoinGame", gameCode);

        GameState = SnakeGameState.GameLobby;
    }

    private async void readyButton_Click(object sender, EventArgs e)
    {
        await _gameClient.PlayerReady(currentGameCodeLabel.Text, playerNameTextBox.Text);
        readyButton.Visible = false;
    }

    private void RefreshGameState()
    {
        mainMenuPanel.Visible = GameState == SnakeGameState.MainMenu;
        joinPanel.Visible = GameState == SnakeGameState.NewGame || GameState == SnakeGameState.JoinGame;
        gameCodeTextBox.Visible = gameCodeLabel.Visible = GameState == SnakeGameState.JoinGame;
        lobbyButton.Text = GameState == SnakeGameState.NewGame ? "CREATE GAME" : "JOIN GAME";
        lobbyPanel.Visible = GameState == SnakeGameState.GameLobby;
    }
}