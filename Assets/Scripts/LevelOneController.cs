using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelOneController : MonoBehaviour
{
    [Header("Porta")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] closedDoorTiles;
    [SerializeField] private TileBase[] openDoorTiles;
    [SerializeField] private Vector3Int doorOrigin;

    private bool doorOpen = false;

    public void OpenDoor()
    {
        if (doorOpen)
            return;

        doorOpen = true;

        int index = 0;

        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                Vector3Int position = doorOrigin + new Vector3Int(x, y, 0);
                tilemap.SetTile(position, openDoorTiles[index]);
                index++;
            }
        }
    }

    public bool IsDoorOpen()
    {
        return doorOpen;
    }
}