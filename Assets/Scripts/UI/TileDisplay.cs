using UnityEngine;

public class TileDisplay : MonoBehaviour
{
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

    private GameObject[,] tileObjects; 

    public void UpdateTiles(Tile[,] tiles)
    {
        if (tileObjects != null)
        {
            CleanUpTiles();
        }
        
        tileObjects = new GameObject[tiles.GetLength(0), tiles.GetLength(1)];
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                GameObject parentObject = new GameObject($"Cell ({i}, {j})");
                GameObject uiObject = new GameObject($"UI");
                SpriteRenderer uiSpriteRenderer = uiObject.AddComponent<SpriteRenderer>();
                uiSpriteRenderer.sprite = tileUi;
                Color tileColor = (tiles[i, j].State) switch
                {
                    TileUIState.Selectable => selectColor,
                    TileUIState.CanAttack => actColor,
                    TileUIState.CanMove => moveColor,
                    _ => Color.white

                };
                uiSpriteRenderer.color = tileColor;

                uiObject.transform.parent = parentObject.transform;
                
                GameObject backgroundObject = new GameObject($"BackGround");
                SpriteRenderer bgSpriteRenderer = backgroundObject.AddComponent<SpriteRenderer>();
                //bgSpriteRenderer.sprite = tileBackground;

                backgroundObject.transform.parent = parentObject.transform;


                parentObject.transform.position = grid.GetCellCenterWorld(new Vector3Int(i, j, 0));

                tileObjects[i, j] = parentObject;
            }
        }
    }

    private void CleanUpTiles()
    {
        for (int i = 0; i < tileObjects.GetLength(0); i++)
        {
            for (int j = 0; j < tileObjects.GetLength(1); j++)
            {
                Destroy(tileObjects[i, j]);
            }
        }
    }
}
