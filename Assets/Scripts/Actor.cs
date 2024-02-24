using System.Collections.Generic;
using System.Numerics;

public class Actor
{
    public string Id { get; private set; }
    public ActorType Type { get; private set; }

    public Vector3 Position { get; private set; }
    public string Name { get; private set; }

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxEnergy { get; private set; }
    public int CurrentEnergy { get; private set; }

    public int Movement { get; private set; }

    public IList<Ability> Abilities { get; private set; }

    public IList<string> Tags { get; private set; }
    public Actor(string name, int health, int energy, int movement, IList<Ability> abilities, IList<string> tags)
    {
        Name = name;
        CurrentHealth = MaxHealth = health;
        CurrentEnergy = MaxEnergy = energy;
        Movement = movement;
        Abilities = new List<Ability>(abilities);
        Tags = new List<string>(tags);

    }
    public void Spawn(string id, ActorType type, Vector3 startingPos)
    {
        Id = id;
        Type = type;
        Position = startingPos;
    }

    public void Move(Vector3 moveTo)
    {
        Position = moveTo;
    }



}