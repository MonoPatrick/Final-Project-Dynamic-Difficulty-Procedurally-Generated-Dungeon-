using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node2D : MonoBehaviour
{
    public Vector2Int dimensions;
    public Vector2Int start;
    public Vector2Int previousRoom;
    private List<List<string>> dungeon = new List<List<string>>(); // example with int cells

    public GameObject pathTilePrefab;
    public GameObject startTilePrefab;
    public GameObject enemyTilePrefab;
    public GameObject endTilePrefab;
    public GameObject wallTilePrefab;

    public int critialPathLength = 13;
    private List<Vector2Int> pathRooms = new List<Vector2Int>();

    private List<List<Vector2Int>> roomDirections = new List<List<Vector2Int>>();
    public RoomTemplates templates;
    void Start()
    {
        initialiseDungeon();
        placeEntrance();
        generateCriticalPath(start, critialPathLength, 0);
        PrintDungeon();
        overwriteNodes();
        PrintDungeon();
        PlacePathObjects();

    }

    void initialiseDungeon()
    {

        dungeon.Clear();
        for (int x = 0; x < dimensions.x +1; x++)
        {
            dungeon.Add(new List<string>());
            for (int y = 0; y < dimensions.y +1; y++)
            {
                dungeon[x].Add("0"); // initialize cell to 0 (or whatever you want)
            }
        }
        Debug.Log("Dungeon initialized with " + dimensions.x + "x" + dimensions.y);
        roomDirections.Clear();
        for (int x = 0; x < critialPathLength; x++)
        {
            roomDirections.Add(new List<Vector2Int>());

            for (int y = 0; y < dimensions.y + 1; y++)
            {
                roomDirections[x].Add(Vector2Int.zero);
            }
                

        }
    }

    void placeEntrance()
    {
        if (start.x < 0 || start.x >= dimensions.x-1)
        {
            start.x = UnityEngine.Random.Range(0, dimensions.x);
            return;
        }
        if (start.y < 0 || start.y >= dimensions.y-1)
        {
            start.y = UnityEngine.Random.Range(0, dimensions.y);
            return;
        }
        dungeon[start.x][start.y] = "s";
        pathRooms.Clear();
        pathRooms.Add(start);
    }
    bool generateCriticalPath(Vector2Int current, int length, int path)
    {
        if (length == 0)
        {
            return true; //
        }
        
        Vector2Int direction;

        int value = UnityEngine.Random.Range(0, 4);

        switch (value)
        {
            // sets the starting direction of the dungeon using the random value generated
            case 0:
                direction = Vector2Int.up; // up == (0,1)
                break;
            case 1:
                direction = Vector2Int.right; // right == (1,0)
                break;
            case 2:
                direction = Vector2Int.down; // down  == (0,-1)
                break;
            case 3:
                direction = Vector2Int.left; // left == (-1,0)
                break;
            default:
                direction = Vector2Int.up;
                break;
        }
        //Debug.Log(direction);
        //
        for (int i = 0; i < 4; i++)
        {
            if (
               current.x + direction.x >= 0 &&
               current.x + direction.x < dimensions.x-1 &&
               current.y + direction.y >= 0 &&
               current.y + direction.y < dimensions.y-1 &&
               dungeon[current.x + direction.x][current.y + direction.y] == "0"
            )
            {
                current += direction;
                dungeon[current.x][current.y] = length.ToString();
                roomDirections[current.x][current.y] = direction;
                pathRooms.Add(current);
                if (generateCriticalPath(current, length - 1, path +1))
                {
                    return true;

                }
                else
                {
                    dungeon[current.x][current.y] = "0";
                    current -= direction;
                }
            }


            direction = new Vector2Int(-direction.y, direction.x); // counter-clockwise

        }

        return false;
    }
    void PrintDungeon()
    {
        string dungeonAsString = "";
        string roomDirectionsAsString = "";

        for (int y = dimensions.y ; y >= 0; y--) // print top row first
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                    dungeonAsString += $"[{dungeon[x][y]}]";
            }
            dungeonAsString += "\n"; // newline after each row
        }
        for (int y = dimensions.y; y >= 0; y--) // print top row first
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                roomDirectionsAsString += $"[{roomDirections[x][y]}]";
            }
            dungeonAsString += "\n"; // newline after each row
        }


        Debug.Log(dungeonAsString);
        Debug.Log(roomDirectionsAsString);
    }
    void overwriteNodes()
    {
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                string cell = dungeon[x][y];
                if (cell != "0" && cell != "s")
                {
                    if (cell == "1")
                    {
                        dungeon[x][y] = "f";
                    }
                    else
                    {
                        dungeon[x][y] = "R";
                    }
                }
                //set room types to R transform the room
                if (dungeon[x][y] == "R")
                {
                    int value = UnityEngine.Random.Range(0, 3);

                    switch (value)
                    {
                        case 0:
                            dungeon[x][y] = "e";
                            break;
                        case 1:
                            dungeon[x][y] = "t";
                            break;
                        case 2:
                            dungeon[x][y] = "p"; 
                            break;

                        default:
                            dungeon[x][y] = "R"; 
                            break;
                    }
                }

            }
        }
    }
    void PlacePathObjects()
    {
        for (int i = 0; i < pathRooms.Count; i++)
        {
            Vector2Int current = pathRooms[i];
            Vector2Int prev;
            if (i > 0)
            {
                prev = pathRooms[i - 1];
            }
            else
            {
                prev = current;
            }
            Vector2Int next;
            if (i < pathRooms.Count - 1)
            {
                next = pathRooms[i + 1];
            }
            else
            {
                next = current;
            }

            Vector2Int entryDir = -(current - prev); // gives the direction from previous to current
            Vector2Int exitDir = (next - current); // exitDir = direction you leave the room toward.

            Vector3 position = new Vector3(current.x * 5, current.y * 5, 0);

            string cell = dungeon[current.x][current.y];
            GameObject room = GetRoomFromTemplates(entryDir, exitDir);


            if (room != null)
            {
                Instantiate(room, position, Quaternion.identity);
            }

            // Start & End override
            if (cell == "s")
            {
                Instantiate(startTilePrefab, position, Quaternion.identity);
                
            }
            if (cell == "e")
            {
                Instantiate(enemyTilePrefab, position, Quaternion.identity);
                
            }

            if (cell == "f")
            {
                Instantiate(endTilePrefab, position, Quaternion.identity);
                
            }   
        }
    }
    GameObject GetRoomFromTemplates(Vector2Int entry, Vector2Int exit)
    {
        // STRAIGHT ROOMS
        if ((entry == Vector2Int.up && exit == Vector2Int.down) ||
            (entry == Vector2Int.down && exit == Vector2Int.up))
        {
            return templates.TopBottomRooms[0];
        }

        if ((entry == Vector2Int.left && exit == Vector2Int.right) ||
            (entry == Vector2Int.right && exit == Vector2Int.left))
        {
            return templates.LeftRightRooms[0];
        }

        // CORNERS
        if ((entry == Vector2Int.up && exit == Vector2Int.right) ||
            (entry == Vector2Int.right && exit == Vector2Int.up))
        {
            return templates.TopRightRooms[0];
        }

        if ((entry == Vector2Int.up && exit == Vector2Int.left) ||
            (entry == Vector2Int.left && exit == Vector2Int.up))
        {
            return templates.TopLeftRooms[0];
        }

        if ((entry == Vector2Int.down && exit == Vector2Int.right) ||
            (entry == Vector2Int.right && exit == Vector2Int.down))
        {
            return templates.BottomRightRooms[0];
        }

        if ((entry == Vector2Int.down && exit == Vector2Int.left) ||
            (entry == Vector2Int.left && exit == Vector2Int.down))
        {
            return templates.BottomLeftRooms[0];
        }

        // DEAD ENDS (optional)
        if (exit == Vector2Int.up)
            return templates.TopRooms[0];

        if (exit == Vector2Int.down)
            return templates.BottomRooms[0];

        if (exit == Vector2Int.left)
            return templates.LeftRooms[0];

        if (exit == Vector2Int.right)
            return templates.RightRooms[0];

        return null;
    }
}
