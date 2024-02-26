using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ActorPositionData
{
    [SerializeField]
    public Vector2Int Position;

    [SerializeField]
    public ActorConfig ActorConfig;
}
