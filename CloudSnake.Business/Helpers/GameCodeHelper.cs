﻿using System.Text;

namespace CloudSnake.Business.Helpers;

public class GameCodeHelper
{
    private static readonly char[] _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private static readonly Random _randomGenerator = new Random();

    public string GenerateGameCode()
    {
        var gameCodeBuilder = new StringBuilder(6);

        for (int i = 0; i < gameCodeBuilder.Capacity; i++)
        {
            gameCodeBuilder.Append(_characters[_randomGenerator.Next(_characters.Length)]);
        }

        return gameCodeBuilder.ToString();
    }
}