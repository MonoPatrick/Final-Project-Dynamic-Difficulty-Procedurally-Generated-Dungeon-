using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    public Vector2Int dimensions;
    public Vector2Int start;

    public int critialPathLength;

    public List<List<string>> dungeon = new List<List<string>>(); // example with int cells
    public List<Vector2Int> criticalPathRooms = new List<Vector2Int>();
    public List<List<Vector2Int>> branchPaths = new List<List<Vector2Int>>();
    public List<List<Vector2Int>> roomDirections = new List<List<Vector2Int>>();
    public Vector2Int previousRoom;
    //difficulty
    public DynamicDifficultyAdjustment DDA;

    //branched paths
    public int branches;
    Vector2Int branchLength = new Vector2Int(3, 5);
    public List<Vector2Int> branchCandidates;


    int keyRoom = 0;
    int availableKeyRoom = 1;

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
        
        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank1)
        {
            critialPathLength = 5;
        }
        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank2)
        {
            critialPathLength = 10;
        }
        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank3)
        {
            critialPathLength = 15;
        }
        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank4)
        {
            critialPathLength = 20;
        }
        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank5)
        {
            critialPathLength = 25;
        }
       

        Debug.Log("Critical Path Length set to: " + critialPathLength);
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
        criticalPathRooms.Clear();
        criticalPathRooms.Add(start);
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
            Vector2Int next = current + direction;
            if (
                current.x + direction.x >= 0 && current.x + direction.x < dimensions.x - 1 &&
                current.y + direction.y >= 0 && current.y + direction.y < dimensions.y - 1 &&
                dungeon[current.x + direction.x][current.y + direction.y] == "0"
            )
            {
                
                
                dungeon[next.x][next.y] = length.ToString();
                branchCandidates.Add(next);
               
                //roomDirections[current.x][current.y] = direction;

                criticalPathRooms.Add(next);

                if (generateCriticalPath(next, length - 1, path + 1))
                {
                    return true;

                }
                else
                {
                    dungeon[next.x][next.y] = "0";
                    criticalPathRooms.RemoveAt(criticalPathRooms.Count - 1);
                    branchCandidates.Remove(next);
                    
                }
            }


            direction = new Vector2Int(-direction.y, direction.x); // counter-clockwise

        }

        return false;
    }

    public List<Vector2Int> GenerateBranch1(Vector2Int start, int length)
    {
        List<Vector2Int> branch = new List<Vector2Int>();
        Vector2Int current = start;

        for (int i = 0; i < length; i++)
        {
            bool placed = false;
            Vector2Int direction = Vector2Int.zero;

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

            Vector2Int next = current + direction;

            if (
                current.x + direction.x >= 0 && current.x + direction.x < dimensions.x - 1 &&
                current.y + direction.y >= 0 && current.y + direction.y < dimensions.y - 1 &&
                dungeon[next.x][next.y] == "0" && IsValidBranchTile(next)
            )
            {
                

                dungeon[next.x][next.y] = "B";// B = branch
                if (i == 0)
                {
                    dungeon[next.x][next.y] = "B0"; // B0 = start of branch
                }
                if (i == length - 1)
                {
                    dungeon[next.x][next.y] = "k" + i.ToString(); // B0 = start of branch
                }
                branch.Add(next);

                current = next;
                placed = true;
                break;
            }
            if (!placed)
            {
                // failed to place this step whole branch fails
                return new List<Vector2Int>();
            }




        }

        return branch;
    }
    bool IsValidBranchTile(Vector2Int pos)
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2Int direction = Vector2Int.zero;

            switch (i)
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
            }

            Vector2Int check = pos + direction;

            if (check.x < 0 || check.x >= dimensions.x ||
                check.y < 0 || check.y >= dimensions.y)
                continue;


            if (dungeon[check.x][check.y] == "s" || dungeon[check.x][check.y] == "f")
            {
                return false;
            }
        }

        return true;
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
    public bool generateBranches()
    {
        branchPaths.Clear();

        branchCandidates.Remove(criticalPathRooms[0]);
        branchCandidates.Remove(criticalPathRooms[criticalPathRooms.Count - 1]);

        int branchCreated = 0;
        int attempts = 0;

        while (branchCreated < branches && branchCandidates.Count > 0 && attempts < branches * 5)
        {
            attempts++;

            int index = Random.Range(0, branchCandidates.Count);
            Vector2Int start = branchCandidates[index];
            branchCandidates.RemoveAt(index);

            List<Vector2Int> branch = new List<Vector2Int>();

            int targetLength = Random.Range(branchLength.x, branchLength.y + 1);

            if (GenerateBranch(start, targetLength, branch))
            {
                branchPaths.Add(branch);
                branchCreated++;
            }
        }

        return branchCreated == branches;
    }
    public bool GenerateBranch(Vector2Int current, int length, List<Vector2Int> branch)
    {
        if (length == 0)
            return true;

        Vector2Int direction = Vector2Int.zero;

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

        int startDir = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
        {
            //Vector2Int direction = directions[(startDir + i) % 4];
            Vector2Int next = current + direction;

            if (
                next.x >= 0 && next.x < dimensions.x - 1 &&
                next.y >= 0 && next.y < dimensions.y - 1 &&
                dungeon[next.x][next.y] == "0" &&
                IsValidBranchTile(next)
            )
            {
                dungeon[next.x][next.y] = "B";

                if (length == 1)
                {
                    if (keyRoom != availableKeyRoom)
                    {
                        dungeon[next.x][next.y] = "BK"; // Branch End
                        keyRoom += 1;
                    }
                    
                }
                else if (branch.Count == 0)
                {
                    
                    dungeon[next.x][next.y] = "B0"; // Branch Start
                }
                else
                {
                    dungeon[next.x][next.y] = "B";  // Middle
                }

                branch.Add(next);

                if (GenerateBranch(next, length - 1, branch))
                    return true;

                // backtrack
                dungeon[next.x][next.y] = "0";
                branch.RemoveAt(branch.Count - 1);

            }
            direction = new Vector2Int(-direction.y, direction.x); // counter-clockwise
        }
       

        return false;
    }


}

    



 
