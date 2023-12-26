using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] private float[] chances;

    private void Start()
    {
        float rnd = Random.Range(0, 1f);

        float rightLimit = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            if (i == 0)
            {
                rightLimit = 0;
            }
            else
            {
                rightLimit += chances[i - 1];
            }

            if(rightLimit < rnd && rnd < rightLimit + chances[i])
            {
                Instantiate(objects[i], transform);
                break;
            }
        }
    }
}
