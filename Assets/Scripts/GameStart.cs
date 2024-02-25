using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{


    //public Start(MapConfig map, IList<ActorConfig> actorConfigs, )
    //{

    //}
    public GameManager Manager { get; private set; }
    void Awake()
    {
        Manager = new GameManager();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
