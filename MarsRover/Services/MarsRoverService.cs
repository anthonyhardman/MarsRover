using MarsRover.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Services
{
    public class MarsRoverService
    {
        private readonly HttpClient http;

        public MarsRoverService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<JoinGameResponse> JoinGame(string gameId, string name)
        {
            var response = await http.GetFromJsonAsync<JoinGameResponse>($"game/join?gameId={gameId}&name={name}");
            return response;
        }

        public async Task<MoveResponse> Move(string token, string direction)
        {
            try
            {
                return await http.GetFromJsonAsync<MoveResponse>($"game/moveperseverance?token={token}&direction={direction}");
            }
            catch 
            {
                return new MoveResponse
                {
                    orientation = direction,
                    message = "Too Many Requests"
                };
            }
        }

        public async Task<String> GameStatus(string token)
        {
            try
            {
                var response = await http.GetFromJsonAsync<GameStatusResponse>($"game/status?token={token}");
                return response.status;
            }
            catch
            {
                return "Invalid";
            }
        }
    }


}
