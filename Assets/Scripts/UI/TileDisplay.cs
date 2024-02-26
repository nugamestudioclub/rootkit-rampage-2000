using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDisplay : MonoBehaviour
{
    /*
     * TODO: 
     * - Change the logic of this class to mutate objects instead of repopulating each time
     * - Make sure ordering of sprite layers is set
     */


    [SerializeField]
    private Grid grid;
    public Grid Grid => grid;

    [SerializeField]
    private Color selectColor;
    [SerializeField]
    private Color moveColor;
    [SerializeField]
    private Color actColor;

    [SerializeField]
    private Sprite tileUi;
    [SerializeField]
    private Sprite tileBackground;

    private GameObject[,] parentObjects;

    //these are parallel arrays
    [SerializeField]
    private List<GameObject> actorPrefabs;
    [SerializeField]
    private List<string> actorNames;

    private GameObject[,] actorObjects;

    private GameObject GetTileObject(int x, int y)
    {
        return parentObjects[x, y].transform.GetChild(0).gameObject;
    }

    private GameObject GetActorObject(int x, int y)
    {
        return parentObjects[x, y].transform.GetChild(1).gameObject;
    }

    public void UpdateActors(IEnumerable<KeyValuePair<Actor, Vector2Int>> actorPositions)
    {
        //will want to support movement/actions
        //so it may make more sense to do this through an "process effect" method
        //TODO add helpers for getting the position of a center tile
        if (parentObjects != null)
        {
            CleanUpActors();
        }
        foreach (KeyValuePair<Actor, Vector2Int> actorPos in actorPositions)
        {
            GameObject parentObject = GetActorObject(actorPos.Value.x, actorPos.Value.y);
            //get actor type from the pair, then spawn the prefab at the
            //same index of that type in the types array
            string name = actorPos.Key.Name;
            int actorIndex = actorNames.IndexOf(name);

            if (actorIndex < 0 || actorIndex >= actorPrefabs.Count)
            {
                Debug.Log($"Could not find actor at index: {actorIndex}, looking for {name}");
                foreach(string an in actorNames)
                {
                    Debug.Log($"{an}");
                }
            }
            else
            {
                GameObject actorPrefab = actorPrefabs[actorIndex];
                GameObject instance = Instantiate(actorPrefab, parentObject.transform);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localScale = Vector3.one;
            }
            

        }
    }
    public void InitializeMapDimension(int width, int height)
    {
        if (parentObjects != null)
        {
            ClearDisplay();
        }

        parentObjects = new GameObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject parentObject = new GameObject($"Cell ({i}, {j})");

                GameObject tilesObject = new GameObject($"Tiles");
                tilesObject.transform.parent = parentObject.transform;

                GameObject actorObject = new GameObject($"Actors");
                actorObject.transform.parent = parentObject.transform;

                parentObject.transform.position = grid.GetCellCenterWorld(new Vector3Int(i, j, 0));
                parentObject.transform.localScale = new Vector3(0.9f, 0.9f, 0);
                parentObjects[i, j] = parentObject;
            }
        }
    }

    public void UpdateTiles(Tile[,] tiles)
    {
        if (parentObjects != null)
        {
            CleanUpTiles();
        }

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                GameObject parentObject = GetTileObject(i, j);
                GameObject uiObject = new GameObject($"UI");
                SpriteRenderer uiSpriteRenderer = uiObject.AddComponent<SpriteRenderer>();
                uiSpriteRenderer.sprite = tileUi;
                Color tileColor = (tiles[i, j].State) switch
                {
                    TileUIState.Selectable => selectColor,
                    TileUIState.CanAttack => actColor,
                    TileUIState.CanMove => moveColor,
                    _ => new Color(0, 0, 0, 0)

                };
                uiSpriteRenderer.color = tileColor;

                uiObject.transform.parent = parentObject.transform;
                uiObject.transform.localPosition = Vector3.zero;
                uiObject.transform.localScale = Vector3.one;

                GameObject backgroundObject = new GameObject($"BackGround");
                SpriteRenderer bgSpriteRenderer = backgroundObject.AddComponent<SpriteRenderer>();
                bgSpriteRenderer.sprite = tileBackground;

                backgroundObject.transform.parent = parentObject.transform;
                backgroundObject.transform.localPosition = Vector3.zero;
                backgroundObject.transform.localScale = Vector3.one;
            }
        }
    }

    //update tiles

    //update actors
    private void CleanUpTiles()
    {
        for (int i = 0; i < parentObjects.GetLength(0); i++)
        {
            for (int j = 0; j < parentObjects.GetLength(1); j++)
            {
                Transform parentTransform = parentObjects[i, j].transform;
                if (parentTransform.childCount > 0)
                {
                    DestroyChildren(parentObjects[i, j].transform.GetChild(0).gameObject);
                }

            }
        }
    }

    private void CleanUpActors()
    {
        for (int i = 0; i < parentObjects.GetLength(0); i++)
        {
            for (int j = 0; j < parentObjects.GetLength(1); j++)
            {
                Transform parentTransform = parentObjects[i, j].transform;
                if (parentTransform.childCount > 1)
                {
                    DestroyChildren(parentObjects[i, j].transform.GetChild(1).gameObject);
                }
            }
        }
    }

    private void ClearDisplay()
    {
        for (int i = 0; i < parentObjects.GetLength(0); i++)
        {
            for (int j = 0; j < parentObjects.GetLength(1); j++)
            {
                Destroy(parentObjects[i, j]);
            }
        }
    }

    private void DestroyChildren(GameObject go)
    {
        for (int i = 0; i < go.transform.childCount; ++i)
        {
            var child = go.transform.GetChild(i).gameObject;

            Destroy(child);

        }
    }
}
