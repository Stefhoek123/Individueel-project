﻿namespace Backend.Hubs;

public interface IChatClient
{
    Task ReceiveMessage(string message);
}