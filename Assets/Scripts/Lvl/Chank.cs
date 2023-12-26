using UnityEngine;

public class Chank : MonoBehaviour
{
    // 0 - чанк в одну платформу.
    // 1 - обычный чанк.
    public int type = 0;

    public Transform start;
    public Transform end;

    public float limitLeft;
    public float limitRight;
}
