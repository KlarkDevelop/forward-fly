using System.Collections.Generic;
using UnityEngine;

public class ChankGenerator : MonoBehaviour
{
    [Header("Chanks")]
    [SerializeField] private Chank[] ChanksPrefubs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform destroyPoint;
    public Chank startChank;
    private List<Chank> chanksInScene = new List<Chank>();

    [Header("Backgrounds")]
    [SerializeField] private float intervalSpawnYLower;
    [SerializeField] private float intervalSpawnYUpper;
    [SerializeField] private float limitX;
    [SerializeField] private GameObject[] backgroundPrefubs;
    public GameObject StartBackground;
    private List<GameObject> backgroundInScene = new List<GameObject>();
    private float spawnY = 0;

    [Header("Wolls")]
    public Woll WollPrefub;
    public Woll StartWoll;
    private List<Woll> wollsInScene = new List<Woll>();

    [Header("Bonuses")]
    [SerializeField] private GameObject healthPrefub;
    [SerializeField] private float intervalSpawnHealthY;
    private float healthOldY = 0;
    [SerializeField] private GameObject shieldPrefub;
    [SerializeField] private float chanceSpawnShield;
    [SerializeField] private float intervalSpawnShieldY;
    private float shieldOldY = 0;
    [SerializeField] private GameObject bombPrefub;
    [SerializeField] private float chanceSpawnBomb;
    [SerializeField] private float intervalSpawnBombY;
    private float bombOldY = 0;
    [SerializeField] private GameObject coinPrefub;
    [SerializeField] private float chanceSpawnCoin;
    [SerializeField] private float intervalSpawnCoinY;
    private float coinOldY = 0;

    private void Start()
    {
        backgroundInScene.Add(StartBackground);
        chanksInScene.Add(startChank);
        wollsInScene.Add(StartWoll);

        bombOldY = intervalSpawnBombY;
        shieldOldY = intervalSpawnShieldY;
        coinOldY = intervalSpawnCoinY;
        oldPosType1 = intervalSpavnType1;
        SortChanks();
    }

    private void Update()
    {
        if (chanksInScene[chanksInScene.Count - 1].end.position.y < spawnPoint.position.y)
        {
            SpawnChank();
        }
        if(chanksInScene[0].end.position.y < destroyPoint.position.y)
        {
            Destroy(chanksInScene[0].gameObject);
            chanksInScene.RemoveAt(0);
        }

        if (wollsInScene[wollsInScene.Count - 1].end.position.y < spawnPoint.position.y)
        {
            SpawnWoll();
        }
        if (wollsInScene[0].end.position.y + 1 < destroyPoint.position.y)
        {
            Destroy(wollsInScene[0].gameObject);
            wollsInScene.RemoveAt(0);
        }

        if(backgroundInScene[backgroundInScene.Count-1].transform.position.y + spawnY < spawnPoint.position.y)
        {
            SpawnBack();
            spawnY = Random.Range(intervalSpawnYLower, intervalSpawnYUpper);
        }
        if(backgroundInScene[0].transform.position.y < destroyPoint.position.y - 20)
        {
            Destroy(backgroundInScene[0]);
            backgroundInScene.RemoveAt(0);
        }
        
    }

    List<Chank> ChankType0 = new List<Chank>();
    List<Chank> ChankType1 = new List<Chank>();
    private void SortChanks()
    {
        foreach(Chank chank in ChanksPrefubs)
        {
            switch (chank.type)
            {
                case 0:
                    ChankType0.Add(chank); break;
                case 1: 
                    ChankType1.Add(chank); break;
            }
        }
    }

    [SerializeField] private float chanceSpawnType1 = 0.5f;
    [SerializeField] private float intervalSpavnType1;
    private float oldPosType1;

    private bool bonusIsSpawned = false;
    private void SpawnChank() 
    {
        bonusIsSpawned = false;

        Chank newChank;
        if ( (oldPosType1 < spawnPoint.position.y - intervalSpavnType1) && (Random.Range(0, 1f) <= chanceSpawnType1) )
        {
            newChank = Instantiate(ChankType1[Random.Range(0, ChankType1.Count)]);
        }
        else
        {
            newChank = Instantiate(ChankType0[Random.Range(0, ChankType0.Count)]);
        }
        newChank.transform.position = new Vector3(Random.Range(newChank.limitLeft, newChank.limitRight), spawnPoint.position.y, 0);
        chanksInScene.Add(newChank);

        if(newChank.type == 1)
        {
            oldPosType1 = newChank.transform.position.y;
        }

        if (healthOldY < spawnPoint.position.y - intervalSpawnHealthY)
        {
            Instantiate(healthPrefub, newChank.start.position, Quaternion.identity, newChank.transform);
            healthOldY = newChank.start.position.y;
            bonusIsSpawned = true;
        }
        if (coinOldY < spawnPoint.position.y - intervalSpawnCoinY && bonusIsSpawned == false)
        {
            if (Random.Range(0, 100f) <= chanceSpawnCoin)
                Instantiate(coinPrefub, newChank.start.position, Quaternion.identity, newChank.transform);
            coinOldY = newChank.start.position.y;
            bonusIsSpawned = true;
        }
        if (bombOldY < spawnPoint.position.y - intervalSpawnBombY && bonusIsSpawned == false)
        {
            if (Random.Range(0, 100f) <= chanceSpawnBomb)
                Instantiate(bombPrefub, newChank.start.position, Quaternion.identity, newChank.transform);
            bombOldY = newChank.start.position.y;
            bonusIsSpawned = true;
        }
        if (shieldOldY < spawnPoint.position.y - intervalSpawnShieldY && bonusIsSpawned == false)
        {
            if (Random.Range(0, 100f) <= chanceSpawnShield)
                Instantiate(shieldPrefub, newChank.start.position, Quaternion.identity, newChank.transform);
            shieldOldY = newChank.start.position.y;
            bonusIsSpawned = true;
        }
    }

    private void SpawnWoll()
    {
        Woll newWoll = Instantiate(WollPrefub);
        newWoll.transform.position = new Vector3(0, Vector3.Distance(newWoll.start.position, newWoll.transform.position) + wollsInScene[wollsInScene.Count - 1].end.position.y, 0);
        wollsInScene.Add(newWoll);
    }

    private void SpawnBack()
    {
        GameObject newBack = Instantiate(backgroundPrefubs[Random.Range(0, backgroundPrefubs.Length)]);
        newBack.transform.position = new Vector3(Random.Range(-limitX, limitX), spawnPoint.position.y + 15, 10.69f);
        backgroundInScene.Add(newBack);
    }
}
