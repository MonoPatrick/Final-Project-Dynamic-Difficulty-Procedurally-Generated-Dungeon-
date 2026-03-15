using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;

    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;
    public float cameraDistanceFromPlayer;

    private void Awake()
    {
        cameraDistanceFromPlayer = -10f;


    }

    private void LateUpdate()
    {
        Vector3 targetPosition = player.position; // sets camera to player postition
        targetPosition.z = cameraDistanceFromPlayer; // increases of decreases the positons


        transform.position = targetPosition;
    }
}