using VotePreferentiel;
using TUnit.Core;
using TUnit.Assertions;
using TUnit.Assertions.Extensions;

namespace VotePreferentiel.Tests;

public class VoteCounterTests
{
    private readonly IVoteCounter _voteCounter = new VoteCounter();

    [Test]
    public async Task CasNominal_GagnanteEstChloe()
    {
        // Arrange : Données de l'exemple du sujet
        var model = new VoteModel
        {
            Votes = new List<VoteEntry>
            {
                new() { Choices = ["Alice", "Bilal", "Chloé"] },
                new() { Choices = ["Alice", "Chloé", "Bilal"] },
                new() { Choices = ["Bilal", "Chloé", "Alice"] },
                new() { Choices = ["Chloé", "Bilal", "Alice"] },
                new() { Choices = ["Chloé", "Alice", "Bilal"] }
            }
        };

        // Act
        string winner = _voteCounter.GetWinner(model);

        // Assert
        await Assert.That(winner).IsEqualTo("Chloé");
    }

    [Test]
    public async Task UnSeulCandidat_GagnantEstAlice()
    {
        var model = new VoteModel
        {
            Votes = new List<VoteEntry>
            {
                new() { Choices = ["Alice"] }
            }
        };

        string winner = _voteCounter.GetWinner(model);

        await Assert.That(winner).IsEqualTo("Alice");
    }

    [Test]
    public async Task ReportDePreferences_VerifieVictoireChloe()
    {
        var model = new VoteModel
        {
            Votes = new List<VoteEntry>
            {
                new() { Choices = ["Alice", "Bilal", "Chloé", "Diane"] },
                new() { Choices = ["Alice", "Bilal", "Chloé", "Diane"] },
                new() { Choices = ["Alice", "Bilal", "Chloé", "Diane"] },
                new() { Choices = ["Alice", "Bilal", "Chloé", "Diane"] },
                new() { Choices = ["Bilal", "Chloé", "Alice", "Diane"] },
                new() { Choices = ["Bilal", "Chloé", "Alice", "Diane"] },
                new() { Choices = ["Bilal", "Chloé", "Alice", "Diane"] },
                new() { Choices = ["Chloé", "Alice", "Bilal", "Diane"] },
                new() { Choices = ["Chloé", "Alice", "Bilal", "Diane"] },
                new() { Choices = ["Chloé", "Alice", "Bilal", "Diane"] },
                new() { Choices = ["Diane", "Chloé", "Alice", "Bilal"] } // Modifié ici
            }
        };

        string winner = _voteCounter.GetWinner(model);
        await Assert.That(winner).IsEqualTo("Chloé");
    }

    [Test]
    public async Task EgaliteDernierePosition_EliminationSimultaneeEtVictoire()
    {
        var model = new VoteModel
        {
            Votes = new List<VoteEntry>
            {
                new() { Choices = ["Alice", "Chloé", "Bilal"] },
                new() { Choices = ["Bilal", "Chloé", "Alice"] },
                new() { Choices = ["Chloé", "Bilal", "Alice"] },
                new() { Choices = ["Chloé", "Bilal", "Alice"] }
            }
        };

        string winner = _voteCounter.GetWinner(model);
        await Assert.That(winner).IsNotNull();
    }

    [Test]
    public async Task BulletinVide_leveException()
    {
        var model = new VoteModel
        {
            Votes = new List<VoteEntry>
            {
                new() { Choices = new List<string>() }
            }
        };

        // Vérifie qu'une exception est levée pour un bulletin vide
        var action = () => _voteCounter.GetWinner(model);
        await Assert.That(action).ThrowsException();
    }
}