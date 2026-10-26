using System.Net.Http.Json;

namespace VotePreferentiel;

public class VoteProvider
{
    private readonly HttpClient _httpClient;
    private const string JsonUrl = "https://raw.githubusercontent.com/JLou/dotnet-course/refs/heads/main/eval-1/votes.json";

    public VoteProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<VoteModel?> GetVotesAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<VoteModel>(JsonUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la récupération des votes : {ex.Message}");
            throw;
        }
    }
}