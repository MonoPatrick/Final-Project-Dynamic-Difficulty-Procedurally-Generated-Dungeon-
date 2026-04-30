using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraph : MonoBehaviour
{

    public Node nodePrefab;
    public List<Node> nodeList;

    public LayerMask obstacleLayer;

    public NPC_Controller[] npcs;

    bool canDrawGizmos;

    public void CreateNodes(Vector3 center)
    {
        nodeList.Clear();

        // spawn nodes in a grid pattern around the center point using -2.5 from center to 2.5 from the center
        for (float x = -2.5f; x <= 2.5f; x += 1.0f)
        {
            for (float y = -2.5f; y <= 2.5f; y += 1.0f)
            {
                Vector3 spawnPos = center + new Vector3(x, y, 0); // calculate the spawn position for the node based on the center point and the current x and y values in the loop
                Node node = Instantiate(nodePrefab, spawnPos, Quaternion.identity);
                Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.3f, obstacleLayer);
                if (hit != null && hit.CompareTag("Obstacles"))
                {
                    node.isBlocked = true;
                }

                nodeList.Add(node);
            }
        }

        CreateConnections();
    }

    void CreateConnections()
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            for (int ii = i + 1; ii < nodeList.Count; ii++)
            {
                if (nodeList[i].isBlocked || nodeList[ii].isBlocked)
                    continue; // if either node is blocked skip the connection which stops enemies going in this area

                if (Vector2.Distance(nodeList[i].transform.position, nodeList[ii].transform.position) <= 1.5f) // set connections with its distance, if the distance is less than or equal to 0.8f then connect the nodes, this is to prevent diagonal connections and only connect nodes that are close enough
                {
                    ConnectNodes(nodeList[i], nodeList[ii]);
                    ConnectNodes(nodeList[ii], nodeList[i]);
                }
            }
        }
        canDrawGizmos = true;
        SpawnAI();

    }

    void ConnectNodes(Node from, Node to)
    {
        if (from == to)
        {
            return;

        }
        from.connections.Add(to);


    }
    void SpawnAI()
    {
        int enemies = UnityEngine.Random.Range(1, 2);

        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank1)
        {
            enemies = UnityEngine.Random.Range(1, 4);
        }
        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank2)
        {
            enemies = UnityEngine.Random.Range(2, 5);
        }
        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank3)
        {
            enemies = UnityEngine.Random.Range(2, 3);
        }
        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank4)
        {
            enemies = UnityEngine.Random.Range(2, 4);
        }
        if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank5)
        {
            enemies = UnityEngine.Random.Range(3, 4);
        }

        for (int i = 0; i < enemies; i++)
        {
            Node randNode = nodeList[Random.Range(0, nodeList.Count)];
            while (randNode.isBlocked)
            {
                randNode = nodeList[Random.Range(0, nodeList.Count)];
            }
            int randEnemy;
            randEnemy = Random.Range(0, npcs.Length);
            NPC_Controller newNPC = Instantiate(npcs[randEnemy], new Vector3(randNode.transform.position.x, randNode.transform.position.y, -0.01f), Quaternion.identity);
            newNPC.currentNode = randNode;
        }


    }
    /*
    private void OnDrawGizmos()
    {
        if (!canDrawGizmos || nodeList == null) return;

        Gizmos.color = Color.blue;

        for (int i = 0; i < nodeList.Count; i++)
        {
            Node node = nodeList[i];

            foreach (Node connected in node.connections)
            {
                if (connected != null)
                {
                    //Gizmos.DrawLine(node.transform.position, connected.transform.position);
                }
            }

        }


    }
    */
}
