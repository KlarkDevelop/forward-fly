using System.Collections;
using UnityEngine;

public class MovingChank : MonoBehaviour
{
    private Chank _chank;
    public float speed;

    private void Start()
    {
        _chank = GetComponent<Chank>();

        Vector3 start;
        Vector3 end;
        if (Random.Range(0, 1f) < 0.5f)
        {
            start = new Vector3(_chank.limitLeft, transform.position.y);
            end = new Vector3(_chank.limitRight, transform.position.y);
        }
        else
        {
            end = new Vector3(_chank.limitLeft, transform.position.y);
            start = new Vector3(_chank.limitRight, transform.position.y);
        }
        

        StartCoroutine(Move(start, end));
    }

    private IEnumerator Move(Vector3 pointStart, Vector3 pointToMove)
    {
        float t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(pointStart, pointToMove, t);
            t += Time.deltaTime / speed;
            yield return null;
        }

        StartCoroutine(Move(pointToMove, pointStart));
    }
}
