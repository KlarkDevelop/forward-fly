using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform bar;
    [SerializeField] private Player player;

    private void Update()
    {
        bar.localScale = new Vector3(Mathf.Lerp(0.01f, 7.12f, player.health), 1, 1);
    }
}
