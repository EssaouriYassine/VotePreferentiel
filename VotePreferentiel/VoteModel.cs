namespace VotePreferentiel;

public class VoteModel
{
    public List<VoteEntry> Votes { get; set; } = new();
}

public class VoteEntry
{
    public List<string> Choices { get; set; } = new();
}