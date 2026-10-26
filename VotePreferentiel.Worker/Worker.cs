using VotePreferentiel;

namespace VotePreferentiel.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly VoteProvider _voteProvider;

    public Worker(ILogger<Worker> logger, VoteProvider voteProvider)
    {
        _logger = logger;
        _voteProvider = voteProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Tentative de récupération des votes sur GitHub...");

        try
        {
            var voteModel = await _voteProvider.GetVotesAsync();

            if (voteModel != null && voteModel.Votes.Count > 0)
            {
                _logger.LogInformation("Succès ! Nombre de bulletins récupérés : {Count}", voteModel.Votes.Count);
                
                // Affichage d'un exemple du premier bulletin pour vérification
                var firstChoices = voteModel.Votes.First().Choices;
                _logger.LogInformation("Premier bulletin - 1er choix : {FirstChoice}", firstChoices.FirstOrDefault());
            }
            else
            {
                _logger.LogWarning("Le modèle de vote est vide ou null.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur critique lors de la récupération des votes.");
        }
    }
}