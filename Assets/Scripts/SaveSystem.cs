using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class SaveSystem : MonoBehaviour
{
    [Header("Components for save")]
    [SerializeField] private CharacterMeneger _characterMeneger;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private RecordWriter _recordWriter;
    [SerializeField] private TMPro.TMP_Text counterBestRecord;

    [HideInInspector]public SaveData Save = new SaveData();
    public static SaveSystem Instante { get; set; }

    private string path;
    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        path = Path.Combine(Application.persistentDataPath, "Save.json");
#else
        path = Path.Combine(Application.dataPath, "Save.json");
#endif  
        if (File.Exists(path))
        {
            Save = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
        }

        ReadData();
        Instante = this;
    }

    private bool death = false;
    private void Start()
    {
        Player.onDeath.AddListener(UpdateCounterDeath);
        _characterMeneger.loadCharacters();
    }
    private void ReadData()
    {
        if (_characterMeneger != null) 
        {
            _characterMeneger.choosedCharacterId = Save.idChoosedCharacter;
        }
        if(_explosion != null)_explosion.UseUpgradeLvl(Save.lvlExplod, Save.lvlShield);
        if (counterBestRecord != null) counterBestRecord.text = Save.bestRecord.ToString();
    }

    private void WriteData()
    {
        if (_characterMeneger != null) Save.idChoosedCharacter = _characterMeneger.choosedCharacterId;
        if (_recordWriter != null && death == true && _recordWriter.record > Save.bestRecord) Save.bestRecord = _recordWriter.record; 
    }

    public void DoSaveData()
    {
        WriteData();
        File.WriteAllText(path, JsonUtility.ToJson(Save));
    }

    public bool IsNewDayFromLastAdLootBox()
    {
        if(DateTime.Now.Day >= Save.dayGetCharacterFromAdBox + 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateCounterDeath()
    {
        Save.countDeath++;
        if(Save.countDeath >= 4)
        {
            InterstitialAd.Instante.ShowAd();
        }
        DoSaveData();
        death = true;
    }

#if !UNITY_EDITOR && UNITY_ANDROID
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            WriteData();
            File.WriteAllText(path, JsonUtility.ToJson(Save));
        }
    }
#endif

    private void OnApplicationQuit()
    {
        WriteData();
        File.WriteAllText(path, JsonUtility.ToJson(Save));
    }
}

[Serializable]
public class SaveData
{
    public string language = "en";
    public int idChoosedCharacter = 0;
    public float volume = 1;

    public int lvlExplod = 1;
    public int lvlShield = 1;
    public int coins;

    public int countDeath;
    public int countRewordedAd;
    public int dayGetCharacterFromAdBox;

    public int bestRecord;

    public List<int> idOpenedCharacters = new List<int>() { 0 };

    public bool trainingInMenu = false;
    public bool trainingInGame = false;
}
