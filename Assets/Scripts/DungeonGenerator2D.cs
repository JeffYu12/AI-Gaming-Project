using UnityEngine;

public class DungeonGenerator2D : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public int walkSteps = 1000;
    public GameObject floorPrefab;
    public GameObject wallPrefab;

    private bool[,] map;

    void Start()
    {
        Generate();
        InstantiateTiles();
    }

    void Generate()
    {
        map = new bool[width, height];
        Vector2Int pos = new Vector2Int(width / 2, height / 2);
        map[pos.x, pos.y] = true;

        for (int i = 0; i < walkSteps; i++)
        {
            Vector2Int dir = GetRandomDirection();
            pos += dir;
            pos.x = Mathf.Clamp(pos.x, 1, width - 2);
            pos.y = Mathf.Clamp(pos.y, 1, height - 2);
            map[pos.x, pos.y] = true;
        }
    }

    Vector2Int GetRandomDirection()
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0: return Vector2Int.up;
            case 1: return Vector2Int.down;
            case 2: return Vector2Int.left;
            default: return Vector2Int.right;
        }
    }

    void InstantiateTiles()
    {
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Vector3 worldPos = new Vector3(x, y, 0);
            if (map[x, y])
            {
                Instantiate(floorPrefab, worldPos, Quaternion.identity, transform);
            }
            else
            {
                Instantiate(wallPrefab, worldPos, Quaternion.identity, transform);
            }
        }
    }
}
