using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    //TODO: Add map config 12x8 tile grid
    
    [field: SerializeField]
    public List<ActorConfig> ActorConfigs { get; private set; }

    [field: SerializeField]
    public List<AbilityConfig> AbilityConfigs { get; private set; }

    [field: SerializeField]
    public UIManager UIManager { get; private set; }

    public GameManager Manager { get; private set; }
    void Awake()
    {
        IEnumerable<Actor> actors = ActorConfigs.Select((ac) => ac.Generate());
        IEnumerable<Ability> abilities = AbilityConfigs.Select((ac) => ac.Generate());
        //ADD MAP, Abilities, and actors to game state
        Manager = new GameManager(
            new GameState(null), //
            new TurnManager(),
            UIManager,
            actors.ToList(),
            abilities.ToList()
        );
    }

    void Update()
    {
        Manager.Tick();
    }
}
