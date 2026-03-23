using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    private readonly float nodeOffset = 1f;

    public List<Color> colors = new List<Color>();
    public List<NodeData> nodeData = new List<NodeData>();

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        List<NodeData> points = GetCentrePoints(nodeOffset);

        for (int i = 0; i < GetCentrePoints(nodeOffset).Count; i++)
        {
            Gizmos.DrawSphere(GetCentrePoints(nodeOffset)[i].NodePosition, 0.2f);
        }
    }
    private void Awake()
    {
        nodeData = GetCentrePoints(nodeOffset);
    }

    private List<NodeData> GetCentrePoints(float offset)
    {
        List<NodeData> points = new List<NodeData>();

        Vector2 pos = transform.position;

        // Right
        points.Add(new NodeData(pos + Vector2.right * transform.localScale.x * offset / 2f));
        // Left
        points.Add(new NodeData(pos + Vector2.left * transform.localScale.x * offset / 2f));
        // Up
        points.Add(new NodeData(pos + Vector2.up * transform.localScale.y * offset / 2f));
        // Down
        points.Add(new NodeData(pos + Vector2.down * transform.localScale.y * offset / 2f));

        return points;
    }
}