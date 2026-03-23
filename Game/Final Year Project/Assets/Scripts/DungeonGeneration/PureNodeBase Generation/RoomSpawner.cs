using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public RoomObject[] RoomObjects;
    public int roomCount;

    private RoomObject starterObject;
    private readonly Hashtable _occupiedPosition = new();
    // Start is called before the first frame update
    void Start()
    {
        SpawnObjectOnNode();
    }

    private void SpawnObjectOnNode()
    {
        CreateStarterPosition(Vector3.zero);

        int retryCount = 0;

        for (int i = 1; i < roomCount + 1; i++)
        {
            Vector3 offset = GenerateOffsetValue();

            starterObject = Instantiate(RoomObjects[0], Vector3.zero, Quaternion.identity);
            starterObject.transform.position = offset;

            ChangeObjectCOlor(starterObject);

            _occupiedPosition.Add(offset, starterObject.gameObject);
        }
    }

    private void CreateStarterPosition(Vector3 pos)
    {
        int rndIndex = Random.Range (0, RoomObjects.Length);
        starterObject = Instantiate(RoomObjects[rndIndex], pos, Quaternion.identity);
        _occupiedPosition.Add(starterObject.transform.position, starterObject.gameObject);
    }

    private Vector3 GenerateOffsetValue()
    {
        int rndIndex = Random.Range(0, starterObject.nodeData.Count);
        NodeData selectedNode = starterObject.nodeData[rndIndex];

        Vector3 offset = new Vector3(
            starterObject.transform.position.x + selectedNode.NodePosition.x * 2f,
            starterObject.transform.position.y + selectedNode.NodePosition.y * 2f,
            0f
        );

        if (_occupiedPosition.ContainsKey(offset))
        {
            return GenerateOffsetValue();
        }

        return offset;
    }
    private void ChangeObjectCOlor(RoomObject objectToSpawn)
    {
        if (objectToSpawn.colors.Count == 0) return;

        int rndIndex = Random.Range(0, objectToSpawn.colors.Count);
        objectToSpawn.GetComponent<SpriteRenderer>().color = objectToSpawn.colors[rndIndex];
    }
}

/*
 * 
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public RoomObject[] RoomObjects; // Array of room prefabs to spawn
    public int roomCount;            // Total number of rooms to generate

    private readonly Hashtable _occupiedPosition = new(); // Stores positions of spawned rooms
    private List<RoomObject> spawnedRooms = new List<RoomObject>(); // All rooms spawned

    void Start()
    {
        SpawnObjectOnNode();
    }

    private void SpawnObjectOnNode()
    {
        // Spawn the first room at (0,0)
        CreateStarterPosition(Vector2.zero);

        // Spawn the rest of the rooms
        for (int i = 1; i < roomCount; i++)
        {
            // Pick a random existing room to expand from
            RoomObject baseRoom = spawnedRooms[Random.Range(0, spawnedRooms.Count)];

            // Calculate a new valid position based on a node of the base room
            Vector2 offset = GenerateOffsetValue(baseRoom);

            // Pick a random room prefab
            RoomObject newRoom = Instantiate(
                RoomObjects[Random.Range(0, RoomObjects.Length)],
                new Vector3(offset.x, offset.y, 0.03f), // keep Z fixed
                Quaternion.identity
            );

            // Assign random color
            ChangeObjectCOlor(newRoom);

            // Mark the position as occupied
            _occupiedPosition.Add(offset, newRoom.gameObject);

            // Add to spawned rooms list
            spawnedRooms.Add(newRoom);
        }
    }

    private void CreateStarterPosition(Vector2 pos)
    {
        int rndIndex = Random.Range(0, RoomObjects.Length);

        RoomObject starterObject = Instantiate(
            RoomObjects[rndIndex],
            new Vector3(pos.x, pos.y, 0.03f),
            Quaternion.identity
        );

        _occupiedPosition.Add(pos, starterObject.gameObject);
        spawnedRooms.Add(starterObject);
    }

    private Vector2 GenerateOffsetValue(RoomObject baseRoom, int attempts = 0)
    {
        // Safety fallback
        if (attempts > 20)
        {
            return (Vector2)baseRoom.transform.position;
        }

        // Pick a random node from the base room
        int rndIndex = Random.Range(0, baseRoom.nodeData.Count);
        NodeData selectedNode = baseRoom.nodeData[rndIndex];

        // Calculate new position
        Vector2 basePos = (Vector2)baseRoom.transform.position + selectedNode.NodePosition * 2f;

        // If already occupied, try again
        if (_occupiedPosition.ContainsKey(basePos))
        {
            return GenerateOffsetValue(baseRoom, attempts + 1);
        }

        return basePos;
    }

    private void ChangeObjectCOlor(RoomObject objectToSpawn)
    {
        int rndIndex = Random.Range(0, objectToSpawn.colors.Count);
        objectToSpawn.GetComponent<Renderer>().material.color = objectToSpawn.colors[rndIndex];
    }
}
*/ 
