using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform point;
    public float speed = 1;

    private Vector3 startPoint;
    private void Start()
    {
        startPoint = platform.position;
        StartCoroutine(Move(startPoint, point.position));
    }

    private IEnumerator Move(Vector3 pointStart, Vector3 pointToMove)
    {
        float t = 0;
        while(t < 1)
        {
            platform.position = Vector3.Lerp(pointStart, pointToMove, t);
            t += Time.deltaTime / speed;
            yield return null;
        }

        StartCoroutine(Move(pointToMove, pointStart));
    }
}
