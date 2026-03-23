using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node2D : MonoBehaviour
{
    public Vector2Int dimensions;
    public Vector2Int start;
    private List<List<string>> dungeon = new List<List<string>>(); // example with int cells
    public GameObject pathTilePrefab;
    public GameObject startTilePrefab;
    public GameObject endTilePrefab;
    public int critialPathLength = 13;
    void Start()
    {
        initialiseDungeon();
        placeEntrance();
        generateCriticalPath(start, critialPathLength);
        PrintDungeon();
        PlacePathObjects();

    }

    void initialiseDungeon()
    {
        dungeon.Clear();
        for (int x = 0; x < dimensions.x; x++)
        {
            dungeon.Add(new List<string>());
            for (int y = 0; y < dimensions.y; y++)
            {
                dungeon[x].Add("0"); // initialize cell to 0 (or whatever you want)
            }
        }
        Debug.Log("Dungeon initialized with " + dimensions.x + "x" + dimensions.y);
    }
    void placeEntrance()
    {
        if (start.x < 0 || start.x >= dimensions.x)
        {
            start.x = UnityEngine.Random.Range(0, dimensions.x);
            return;
        }
        if (start.y < 0 || start.y >= dimensions.y)
        {
            start.y = UnityEngine.Random.Range(0, dimensions.y);
            return;
        }
        dungeon[start.x][start.y] = "S";
    }
    bool generateCriticalPath(Vector2Int current, int length)
    {
        if (length == 0)
        {
            return true;
        }
        
        Vector2Int direction;

        int value = UnityEngine.Random.Range(0, 4);

        switch (value)
        {
            case 0:
                direction = Vector2Int.up;
                break;
            case 1:
                direction = Vector2Int.right;
                break;
            case 2:
                direction = Vector2Int.down;
                break;
            case 3:
                direction = Vector2Int.left;
                break;
            default:
                direction = Vector2Int.up; // fallback (never actually hit)
                break;
        }
        for (int i = 0; i < 4; i++)
        {
            if (
               current.x + direction.x >= 0 &&
               current.x + direction.x < dimensions.x &&
               current.y + direction.y >= 0 &&
               current.y + direction.y < dimensions.y &&
               dungeon[current.x + direction.x][current.y + direction.y] == "0"
            )
            {
                current += direction;
                dungeon[current.x][current.y] = length.ToString();

                if (generateCriticalPath(current, length - 1))
                {
                    return true;
                }
                else
                {
                    dungeon[current.x][current.y] = "0";
                    current -= direction;
                }
            }

            // rotate direction (same as Godot)
            direction = new Vector2Int(direction.y, -direction.x);
        }

        return false;
    }
    void PrintDungeon()
    {
        string dungeonAsString = "";

        for (int y = dimensions.y - 1; y >= 0; y--) // print top row first
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                    dungeonAsString += $"[{dungeon[x][y]}]";
            }
            dungeonAsString += "\n"; // newline after each row
        }
        

        Debug.Log(dungeonAsString);
    }
    void PlacePathObjects()
    {
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                string cell = dungeon[x][y];
                if (cell != "0") // path or start
                {
                    Vector3 position = new Vector3(x, y, 0); // adjust Z if needed
                    Instantiate(pathTilePrefab, position, Quaternion.identity);
                }
                if (cell == "S")
                {
                    Vector3 position = new Vector3(x, y, 0); // adjust Z if needed
                    Instantiate(startTilePrefab, position, Quaternion.identity);
                }
                if (cell == "1")
                {
                    Vector3 position = new Vector3(x, y, 0); // adjust Z if needed
                    Instantiate(endTilePrefab, position, Quaternion.identity);
                }
            }
        }
    }
}
