using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Actor
{
    public string Id { get; private set; }
    public ActorType Type { get; private set; }

    public Vector2Int Position { get; private set; }
    public string Name { get; private set; }

    public int MaxHealth { get; private set; }

    public int _currentHealth;
    public int CurrentHealth
    {
        get => _currentHealth; 
        set
        {
            _currentHealth = value;
            if (CurrentHealth <=0)
            {
                Debug.Log($"{Id} has died");
                IsDead = true;
            }
        }
    }

    public bool IsDead { get; private set; }
    public int MaxEnergy { get; private set; }
    public int CurrentEnergy { get; private set; }

    public int Movement { get; private set; }

    public IList<Ability> Abilities { get; private set; }

    public IList<string> Tags { get; private set; }
    public Actor(string name, int health, int energy, int movement, IList<Ability> abilities, IList<string> tags)
    {
        Name = name;
        CurrentHealth = MaxHealth = health;
        IsDead = false;
        CurrentEnergy = MaxEnergy = energy;
        Movement = movement;
        Abilities = new List<Ability>(abilities);
        Tags = new List<string>(tags);

    }
    public void Spawn(string id, ActorType type, Vector2Int startingPos)
    {
        Id = id;
        Type = type;
        Position = startingPos;
    }

    public void Move(Vector2Int moveTo)
    {
        Position = moveTo;
    }



}