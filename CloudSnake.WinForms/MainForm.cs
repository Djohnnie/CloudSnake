using Microsoft.AspNetCore.SignalR.Client;

namespace CloudSnake.WinForms;

public partial class MainForm : Form
{
    private readonly string _clientId = $"{Guid.NewGuid()}";
    private bool _toggle = false;

    public MainForm()
    {
        InitializeComponent();
    }

    private HubConnection connection;

    private async void MainForm_Load(object sender, EventArgs e)
    {
        connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7080/ticker")
            .Build();

        connection.Closed += async (error) =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await connection.StartAsync();
        };

        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Invoke(() =>
            {
                BackColor = _toggle ? Color.BlueViolet : Color.DarkOrange;
                _toggle = !_toggle;
            });
        });

        try
        {
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
            Text = ex.Message;
        }
    }

    private async void MainForm_KeyDown(object sender, KeyEventArgs e)
    {
        if (connection.State == HubConnectionState.Connected)
        {
            await connection.SendAsync("Turn", _clientId);
        }
    }

    private void newGameButton_Click(object sender, EventArgs e)
    {

    }

    private void joinGameButton_Click(object sender, EventArgs e)
    {

    }
}