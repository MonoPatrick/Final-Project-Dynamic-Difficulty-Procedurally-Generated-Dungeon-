using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraph : MonoBehaviour
{

    public Node nodePrefab;
    public List<Node> nodeList;


    public NPC_Controller[] npcs;

    bool canDrawGizmos;

    public void CreateNodes(Vector3 center)
           {
                nodeList.Clear();
                for (float x = -1.5f; x <= 1.5f; x += 0.75f)
                {
                    for (float y = -1.5f; y <= 1.5f; y += 0.75f)
                    {
                        Vector3 spawnPos = center + new Vector3(x, y, 0);
                        Node node = Instantiate(nodePrefab, spawnPos, Quaternion.identity);
                        Collider2D[] hits = Physics2D.OverlapCircleAll(spawnPos, 0.2f);

                        foreach (Collider2D hit in hits)
                        {
                            if (hit.CompareTag("Obstacles"))
                            {
                                node.isBlocked = true;
                                Debug.Log("Node blocked by object at: " + spawnPos);
                                break;
                            }
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
                    continue;

                if (Vector2.Distance(nodeList[i].transform.position, nodeList[ii].transform.position) <= 1.2f)
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
        int enemies;

        enemies = UnityEngine.Random.Range(1, 3);
        for (int i = 0; i < enemies; i++)
        {
            Node randNode = nodeList[Random.Range(0, nodeList.Count)];
            while (randNode.isBlocked)
            {
                randNode = nodeList[Random.Range(0, nodeList.Count)];
            }
            int randEnemy;
            randEnemy = Random.Range(0, npcs.Length);
            NPC_Controller newNPC = Instantiate(npcs[1], new Vector3(randNode.transform.position.x, randNode.transform.position.y, -0.01f), Quaternion.identity);
            newNPC.currentNode = randNode;
        }


    }
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
}
