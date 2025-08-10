using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BSPDungeonGenerator : MonoBehaviour
{
    public int mapWidth = 50;
    public int mapHeight = 50;
    public int minRoomSize = 6;
    public int maxRoomSize = 15;
    public int maxSplits = 5;

    public TileBase wallTile;
    public TileBase floorTile;

    public Tilemap wallTilemap;
    public Tilemap floorTilemap;

    private List<RectInt> rooms;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // Fill the map with walls first
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                floorTilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }

        rooms = new List<RectInt>();

        // Start BSP split from full map
        RectInt rootArea = new RectInt(1, 1, mapWidth - 2, mapHeight - 2);
        SplitArea(rootArea, maxSplits);

        // Dig out rooms
        foreach (var room in rooms)
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                for (int y = room.yMin; y < room.yMax; y++)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), null);
                    floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }
        }

        // Connect rooms with corridors
        for (int i = 0; i < rooms.Count - 1; i++)
        {
            Vector2Int roomCenter1 = new Vector2Int(
                (rooms[i].xMin + rooms[i].xMax) / 2,
                (rooms[i].yMin + rooms[i].yMax) / 2
            );

            Vector2Int roomCenter2 = new Vector2Int(
                (rooms[i + 1].xMin + rooms[i + 1].xMax) / 2,
                (rooms[i + 1].yMin + rooms[i + 1].yMax) / 2
            );

            CreateCorridor(roomCenter1, roomCenter2);
        }
    }

    void SplitArea(RectInt area, int depth)
    {
        if (depth <= 0 || area.width < minRoomSize * 2 || area.height < minRoomSize * 2)
        {
            // Make a room inside this area
            int roomWidth = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, area.width));
            int roomHeight = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, area.height));
            int roomX = Random.Range(area.xMin, area.xMax - roomWidth);
            int roomY = Random.Range(area.yMin, area.yMax - roomHeight);

            rooms.Add(new RectInt(roomX, roomY, roomWidth, roomHeight));
            return;
        }

        bool splitVertically = Random.value > 0.5f;

        if (splitVertically && area.width >= minRoomSize * 2)
        {
            int splitX = Random.Range(area.xMin + minRoomSize, area.xMax - minRoomSize);
            RectInt leftArea = new RectInt(area.xMin, area.yMin, splitX - area.xMin, area.height);
            RectInt rightArea = new RectInt(splitX, area.yMin, area.xMax - splitX, area.height);

            SplitArea(leftArea, depth - 1);
            SplitArea(rightArea, depth - 1);
        }
        else if (!splitVertically && area.height >= minRoomSize * 2)
        {
            int splitY = Random.Range(area.yMin + minRoomSize, area.yMax - minRoomSize);
            RectInt bottomArea = new RectInt(area.xMin, area.yMin, area.width, splitY - area.yMin);
            RectInt topArea = new RectInt(area.xMin, splitY, area.width, area.yMax - splitY);

            SplitArea(bottomArea, depth - 1);
            SplitArea(topArea, depth - 1);
        }
        else
        {
            // If can't split, just make a room
            int roomWidth = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, area.width));
            int roomHeight = Random.Range(minRoomSize, Mathf.Min(maxRoomSize, area.height));
            int roomX = Random.Range(area.xMin, area.xMax - roomWidth);
            int roomY = Random.Range(area.yMin, area.yMax - roomHeight);

            rooms.Add(new RectInt(roomX, roomY, roomWidth, roomHeight));
        }
    }

    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        Vector2Int current = start;

        while (current.x != end.x)
        {
            wallTilemap.SetTile(new Vector3Int(current.x, current.y, 0), null);
            floorTilemap.SetTile(new Vector3Int(current.x, current.y, 0), floorTile);
            current.x += (end.x > current.x) ? 1 : -1;
        }

        while (current.y != end.y)
        {
            wallTilemap.SetTile(new Vector3Int(current.x, current.y, 0), null);
            floorTilemap.SetTile(new Vector3Int(current.x, current.y, 0), floorTile);
            current.y += (end.y > current.y) ? 1 : -1;
        }
    }
}
