using Azure;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Services.AccountService;
using Services.FeedbackService;
using Services.JwtService;
using System.Data;

namespace SignalR.NotificationHub
{
    public class NotificationHub : Hub
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IJwtService _jwtService;
        private readonly IAccountService _accountService;
        private List<ConnectedClient> ConnectedClients { get; set; }

        public NotificationHub(IFeedbackService feedbackService, IJwtService jwtService, IAccountService accountService)
        {
            _feedbackService = feedbackService;
            _jwtService = jwtService;
            _accountService = accountService;

            ConnectedClients = new List<ConnectedClient>();
        }

        public override async Task OnConnectedAsync()
        {
            string clientId = Context.ConnectionId; 

            ConnectedClients.Add(new ConnectedClient()
            {
                ClientId = clientId
            });

            Console.WriteLine(clientId);

            await Clients.Client(Context.ConnectionId).SendAsync("Welcome", "Welcome to the notification hub!");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var client = ConnectedClients.FirstOrDefault(c => c.ClientId == Context.ConnectionId);
            if (client != null)
            {
                ConnectedClients.Remove(client);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SignIn(SignInRequest request)
        {
            try
            {
                var response = await _accountService.SignInService(request.Email, request.Password);
                await Clients.Caller.SendAsync("SignInSuccess", response);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("SignInError", ex.Message);
            }
        }
    }
}

public class ConnectedClient
{
    public string ClientId { get; set; } = default!;
    public string? AccountId { get; set; } = default!;
}