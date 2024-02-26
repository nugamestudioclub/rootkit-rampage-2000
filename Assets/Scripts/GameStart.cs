using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    //Could be a list for later levels
    [field: SerializeField]
    public MapConfig MapConfig { get; private set; }

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

        IEnumerable<(Ability, Sprite)> abilities = AbilityConfigs.Select((ac) => ac.Generate());
        (Map startingMap, IEnumerable<KeyValuePair<string, Actor>> actorsToids) = MapConfig.Generate();
        Dictionary<string, Actor> actorDict = new Dictionary<string, Actor>();
        foreach (KeyValuePair<string, Actor> actor in actorsToids)
        {
            actorDict.Add(actor.Key, actor.Value);
        }
        GameState initialGameState = new GameState(startingMap, actorDict);
        UIManager.Load(initialGameState, abilities.Select((a) => 
        new KeyValuePair<AbilityType, Sprite>(a.Item1.Type, a.Item2)));
        //TODO (after jam): Abilities, and actors to game state
        Manager = new GameManager(
            initialGameState,
            new TurnManager(),
            UIManager,
            actors.ToList(),
            abilities.Select((a) => a.Item1).ToList()
        );
        Debug.Log($"Starting Game Mode: {Enum.GetName(typeof(GameMode), initialGameState.CurrentMode)}");
        //for debugging
        initialGameState.CurrentMode = GameMode.WaitingForSelection;
    }

    void Update()
    {
        Manager.Tick();
    }
}
