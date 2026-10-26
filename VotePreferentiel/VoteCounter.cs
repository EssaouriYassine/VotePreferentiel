namespace VotePreferentiel;

public class VoteCounter : IVoteCounter
{
    public HashSet<string> GetCandidates(VoteModel voteModel)
    {
        var candidates = new HashSet<string>();
        foreach (var vote in voteModel.Votes)
        {
            if (vote.Choices != null)
            {
                foreach (var candidate in vote.Choices)
                {
                    candidates.Add(candidate);
                }
            }
        }
        return candidates;
    }

    public List<string> GetAllFirstChoices(VoteModel voteModel)
    {
        var firstChoices = new List<string>();
        foreach (var vote in voteModel.Votes)
        {
            if (vote.Choices != null && vote.Choices.Count > 0)
            {
                firstChoices.Add(vote.Choices[0]);
            }
        }
        return firstChoices;
    }

    public IDictionary<string, int> CountFirstChoices(List<string> firstChoices)
    {
        var counts = new Dictionary<string, int>();
        foreach (var choice in firstChoices)
        {
            if (counts.ContainsKey(choice))
            {
                counts[choice]++;
            }
            else
            {
                counts[choice] = 1;
            }
        }
        return counts;
    }

    public string[] GetCandidatesWithLeastVotes(IDictionary<string, int> firstChoiceCounts)
    {
        if (firstChoiceCounts == null || firstChoiceCounts.Count == 0)
            return Array.Empty<string>();

        int minVotes = firstChoiceCounts.Values.Min();
        return firstChoiceCounts.Where(pair => pair.Value == minVotes)
                                 .Select(pair => pair.Key)
                                 .ToArray();
    }

    public string GetWinner(VoteModel voteModel)
    {
        // Copie des votes pour pouvoir manipuler les listes de préférences (éliminations)
        var ballots = voteModel.Votes
            .Where(v => v.Choices != null && v.Choices.Count > 0)
            .Select(v => new List<string>(v.Choices))
            .ToList();

        if (ballots.Count == 0)
            throw new InvalidOperationException("Aucun bulletin de vote valide trouvé.");

        while (true)
        {
            // Récupérer le premier choix de chaque bulletin actif
            var firstChoices = ballots.Select(b => b[0]).ToList();
            var counts = CountFirstChoices(firstChoices);
            
            int totalVotes = firstChoices.Count;

            // Vérifier si un candidat a la majorité absolue (> 50%) ou s'il ne reste qu'un candidat
            foreach (var kvp in counts)
            {
                if (kvp.Value > totalVotes / 2 || counts.Count == 1)
                {
                    return kvp.Key;
                }
            }

            // Trouver le(s) candidat(s) ayant le moins de voix
            var leastCandidates = GetCandidatesWithLeastVotes(counts);

            // Éliminer ce(s) candidat(s) de tous les bulletins
            foreach (var ballot in ballots)
            {
                ballot.RemoveAll(c => leastCandidates.Contains(c));
            }

            // Sécurité : si tous les bulletins deviennent vides
            ballots.RemoveAll(b => b.Count == 0);
            if (ballots.Count == 0)
            {
                // En cas d'égalité parfaite ou de blocage, on retourne le premier des candidats à égalité
                return leastCandidates.FirstOrDefault() ?? "Inconnu";
            }
        }
    }
}