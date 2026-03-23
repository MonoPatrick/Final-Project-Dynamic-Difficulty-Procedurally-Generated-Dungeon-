using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    /*
    public int amountOfRooms = 10;

    public RoomNode Generate()
    {
        RoomNode start = new RoomNode("Start");
        Expand(start, 0);
        return start;
    }

    void Expand(RoomNode node, int depth)
    {
        if (depth >= maxDepth)
        {
            node.connections.Add(new RoomNode("Boss"));
            return;
        }

        if (node.type == "Start" || node.type == "Combat")
        {
            int branches = Random.Range(1, 3);

            for (int i = 0; i < branches; i++)
            {
                string nextType = GetNextRoomType(depth);
                RoomNode newNode = new RoomNode(nextType);

                node.connections.Add(newNode);
                Expand(newNode, depth + 1);
            }
        }
    }
    string GetNextRoomType(int depth)
    {
        float roll = Random.value;

        if (depth > 3 && roll < 0.3f)
            return "Boss";
        if (roll < 0.6f)
            return "Combat";
        return "Treasure";
    }
}
    */

}
