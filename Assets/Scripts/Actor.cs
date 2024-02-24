public class Actor
{
    public string Id { get; private set; }
    public ActorType type;
    public Actor(string id)
    {
        Id = id;
    }
    
}