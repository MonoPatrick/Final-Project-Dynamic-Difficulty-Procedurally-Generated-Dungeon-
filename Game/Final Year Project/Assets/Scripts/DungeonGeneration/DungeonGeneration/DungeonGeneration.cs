using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public Vector2Int dimensions;
    public Vector2Int start;

    public int critialPathLength;

    public List<List<string>> dungeon = new List<List<string>>(); // example with int cells
    public List<Vector2Int> pathRooms = new List<Vector2Int>();
    public List<List<Vector2Int>> roomDirections = new List<List<Vector2Int>>();
    public Vector2Int previousRoom;
    //difficulty
    public DynamicDifficultyAdjustment DDA;

    //branched paths
    int branches = 0;
    Vector2Int branchLength; //= new Vector2Int(2, 5);
    public List<Vector2Int> branchCandidates;

    public void initialiseDungeon()
    {

        dungeon.Clear();
        for (int x = 0; x < dimensions.x + 1; x++)
        {
            dungeon.Add(new List<string>());
            for (int y = 0; y < dimensions.y + 1; y++)
            {
                dungeon[x].Add("0"); // initialize cell to 0 (or whatever you want)
            }
        }
        Debug.Log("Dungeon initialized with " + dimensions.x + "x" + dimensions.y);
        roomDirections.Clear();
        for (int x = 0; x < dimensions.x; x++)
        {
            roomDirections.Add(new List<Vector2Int>());

            for (int y = 0; y < dimensions.y + 1; y++)
            {
                roomDirections[x].Add(Vector2Int.zero);

            }


        }
        setCriticalPathLength();
    }

    public void setCriticalPathLength()
    {
        if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank1)
        {
            critialPathLength = 5;
        }
        else if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank2)
        {
            critialPathLength = 10;
        }
        else if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank3)
        {
            critialPathLength = 15;
        }
        else if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank4)
        {
            critialPathLength = 20;
        }
        else if (DDA.playerRank == DynamicDifficultyAdjustment.Rank.Rank5)
        {
            critialPathLength = 25;
        }
    }

    public void placeEntrance()
    {
        if (start.x < 0 || start.x >= dimensions.x - 1)
        {
            start.x = UnityEngine.Random.Range(0, dimensions.x);
            return;
        }
        if (start.y < 0 || start.y >= dimensions.y - 1)
        {
            start.y = UnityEngine.Random.Range(0, dimensions.y);
            return;
        }
        dungeon[start.x][start.y] = "s";
        pathRooms.Clear();
        pathRooms.Add(start);
    }

    public bool generateCriticalPath(Vector2Int current, int length, int path)
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
                current.x + direction.x >= 0 && current.x + direction.x < dimensions.x - 1 &&
                current.y + direction.y >= 0 && current.y + direction.y < dimensions.y - 1 &&
                dungeon[current.x + direction.x][current.y + direction.y] == "0"
            )
            {
                current += direction;
                dungeon[current.x][current.y] = length.ToString();
                branchCandidates.Add(current);
                //roomDirections[current.x][current.y] = direction;

                pathRooms.Add(current);

                if (generateCriticalPath(current, length - 1, path + 1))
                {
                    return true;

                }
                else
                {
                    dungeon[current.x][current.y] = "0";
                    pathRooms.RemoveAt(pathRooms.Count - 1);
                    branchCandidates.Remove(current);
                    current -= direction;
                }
            }


            direction = new Vector2Int(-direction.y, direction.x); // counter-clockwise

        }

        return false;
    }
    public void PrintDungeon()
    {
        string dungeonAsString = "";
        string roomDirectionsAsString = "";

        for (int y = dimensions.y; y >= 0; y--) // print top row first
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
    public void generateBranches()
    {
        int branchCreated = 0;
        Vector2Int candidate;
        while(branchCreated < branches && branchCreated < branchCandidates.Count)
        {
            candidate = branchCandidates[Random.Range(0, branchCandidates.Count)];
            if (generateCriticalPath(candidate, Random.Range(branchLength.x, branchLength.y), 0))
            {
                branchCreated++;
            }
            else 
            {
                branchCandidates.Remove(candidate);
            }


            
        }
    }


}
