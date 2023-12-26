using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MainMenuMeneger : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private GameObject eventSystem;

    [SerializeField] private Canvas mainMenu;
    private Vector3 MainMenuPosition;
    [SerializeField] private Canvas CharcterMenu;
    [SerializeField] private Canvas SettingsMenu;
    [SerializeField] private Canvas ChoosedCharacterCanvas;
    [SerializeField] private Transform pointCharacterMenu;
    [SerializeField] private Transform pointSettingsMenu;

    [SerializeField] private Transform choosedCharacterTransform;

    [SerializeField] private AudioClip soundUi;
    [SerializeField] private AudioClip soundError;
    [SerializeField] private AudioClip soundUpgr;
    [SerializeField] private AudioClip soundCharacter;

    private Camera CameraMain;

    public void InitCharacterGenerator(GameObject character)
    {
        chosedCharacterInScene = character;
        SpawnOpenedCharacters();
        SetActiveArrows();
    }
    private AudioSource _audioSource;
    private void Start()
    {
        CameraMain = Camera.main;
        MainMenuPosition = CameraMain.transform.position;
        ReadSettingsData();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        ReadSaveData();
        ChangePrices();
        _audioSource = GetComponent<AudioSource>();
        lootBoxPriceTXT.text = lootBoxPrice.ToString();
    }
    private void ReadSaveData()
    {
        lvlExpl = SaveSystem.Instante.Save.lvlExplod;
        lvlShield = SaveSystem.Instante.Save.lvlShield;
        lvlExplValue.text = lvlExpl.ToString();
        lvlShieldValue.text = lvlShield.ToString();
        GetCoinsValue();
    }

    public void MoveToCharacterMenu()
    {
        StartCoroutine(moveToPointCanvas(pointCharacterMenu.position, 1f, mainMenu, CharcterMenu, chosedCharacterInScene.transform.GetChild(1).gameObject));
    }

    public void MoveToMainMenu()
    {
        IEnumerator routin;
        if(CameraMain.transform.position.y > 0)
        {
            routin = moveToPointCanvas(MainMenuPosition, 1, CharcterMenu, mainMenu);
        }
        else
        {
            routin = moveToPointCanvas(MainMenuPosition, 1, SettingsMenu, mainMenu);
            StartCoroutine(ActiveChoosedCharacterCanvas(routin));
        }
        StartCoroutine(routin);
    }

    public void MoveToSettingsMenu()
    {
        ChoosedCharacterCanvas.renderMode = RenderMode.WorldSpace;
        StartCoroutine(moveToPointCanvas(pointSettingsMenu.position, 1, mainMenu, SettingsMenu));
    }

    private IEnumerator moveToPointCanvas(Vector3 point, float speed, Canvas canvasStart, Canvas canvasEnd)
    {
        eventSystem.SetActive(false);
        canvasStart.renderMode = RenderMode.WorldSpace;
        Vector3 start = CameraMain.transform.position;
        float t = 0;

        while (t < 1)
        {
            CameraMain.transform.position = Vector3.Lerp(start, point, t * t);
            t += Time.deltaTime / speed;
            yield return null;
        }
        CameraMain.transform.position = point;
        canvasEnd.renderMode = RenderMode.ScreenSpaceCamera;
        eventSystem.SetActive(true);
    }
    private IEnumerator moveToPointCanvas(Vector3 point, float speed, Canvas canvasStart, Canvas canvasEnd, GameObject trail)
    {
        trail.SetActive(true);

        yield return moveToPointCanvas(point, speed, canvasStart, canvasEnd);

        trail.SetActive(false);
    }

    private IEnumerator ActiveChoosedCharacterCanvas(IEnumerator routine)
    {
        yield return routine;
        ChoosedCharacterCanvas.renderMode = RenderMode.ScreenSpaceCamera;
    }

    [Header("Character menu")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private Transform choosedCharacterParent;
    [SerializeField] private CharacterMeneger _characterMeneger;
    private GameObject chosedCharacterInScene;
    private List<Transform> prefubsInScen = new List<Transform>();

    public float stepMoveCharacters;
    public float speedMove = 1;

    private void SpawnOpenedCharacters()
    {
        Vector3 erore = new Vector3();

        for(int i = 0; i < _characterMeneger.openedCharacters.Count; i++)
        {
            if(_characterMeneger.openedCharacters[i].id != _characterMeneger.choosedCharacterId)
            {
                Vector3 offset = new Vector3(stepMoveCharacters * i, 0, 0) - erore;
                Transform newPref = Instantiate(_characterMeneger.openedCharacters[i].pref, rightPoint.position + offset , Quaternion.identity, rightPoint).transform;
                prefubsInScen.Add(newPref);
                newPref.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"ID shoosed in array: {i}");
                prefubsInScen.Add(chosedCharacterInScene.transform);
                chosedCharacterInScene.transform.GetChild(1).gameObject.SetActive(false);
                erore = new Vector3(stepMoveCharacters, 0, 0);
            }
        }
    }

    private int idChoosedCharacterInMenu;
    [SerializeField] private GameObject arrowLeft;
    [SerializeField] private GameObject arrowRight;
    public void SwitchCharacters(bool moveLeft)
    {
        for(int i = 0; i < prefubsInScen.Count; i++)
        {
            Vector3 point = new Vector3();

            if (moveLeft)
            {
                if(prefubsInScen[i].position == choosedCharacterParent.position)
                {
                    prefubsInScen[i].transform.parent = leftPoint;
                    point = leftPoint.position;
                }
                else if(prefubsInScen[i].position == rightPoint.position)
                {
                    prefubsInScen[i].transform.parent = choosedCharacterParent;
                    point = choosedCharacterParent.position;
                    idChoosedCharacterInMenu ++;
                }
                else
                {
                    point = new Vector3(prefubsInScen[i].position.x - stepMoveCharacters, leftPoint.position.y, leftPoint.position.z);
                }
            }
            else
            {
                if (prefubsInScen[i].position == choosedCharacterParent.position)
                {
                    prefubsInScen[i].transform.parent = rightPoint;
                    point = rightPoint.position;
                }
                else if (prefubsInScen[i].position == leftPoint.position)
                {
                    prefubsInScen[i].transform.parent = choosedCharacterParent;
                    point = choosedCharacterParent.position;
                    idChoosedCharacterInMenu --;
                }
                else
                {
                    point = new Vector3(prefubsInScen[i].position.x + stepMoveCharacters, rightPoint.position.y, rightPoint.position.z);
                }
            }

            StartCoroutine(MoveToPoint(prefubsInScen[i].transform, point));
        }
        Debug.Log(idChoosedCharacterInMenu);
        SetActiveArrows();
    }

    private IEnumerator MoveToPoint(Transform movingObject , Vector3 point)
    {
        eventSystem.SetActive(false);
        Vector3 start = movingObject.position;
        float t = 0;

        while (t < 1)
        {
            movingObject.position = Vector3.Lerp(start, point, t * t);
            t += Time.deltaTime / speedMove;
            yield return null;
        }
        movingObject.position = point;
        eventSystem.SetActive(true);
    }

    private void SetActiveArrows()
    {
        if(rightPoint.childCount == 0)
        {
            arrowRight.SetActive(false);
        }
        else
        {
            arrowRight.SetActive(true);
        }
        if (leftPoint.childCount == 0)
        {
            arrowLeft.SetActive(false);
        }
        else
        {
            arrowLeft.SetActive(true);
        }
    }

    public void SelectCharacter()
    {
        _characterMeneger.choosedCharacterId = _characterMeneger.openedCharacters[idChoosedCharacterInMenu].id;
        Debug.Log(_characterMeneger.openedCharacters[idChoosedCharacterInMenu].id);
        chosedCharacterInScene = prefubsInScen[idChoosedCharacterInMenu].gameObject;
        MoveToMainMenu();
    }

    private int lvlExplPrice;
    private int lvlShieldPrice;
    private int lvlExpl;
    private int lvlShield;
    [SerializeField] private TMP_Text lvlExplValue;
    [SerializeField] private TMP_Text lvlShieldValue;
    public void UpgradeExplosion()
    {
        if(lvlExpl < 4 && SaveSystem.Instante.Save.coins >= lvlExplPrice)
        {
            _audioSource.PlayOneShot(soundUpgr);
            lvlExpl++;
            lvlExplValue.text = lvlExpl.ToString();
            SaveSystem.Instante.Save.lvlExplod = lvlExpl;
            SaveSystem.Instante.Save.coins -= lvlExplPrice;
            ChangePrices();
            GetCoinsValue();
        }
        else
        {
            Debug.Log("errore");
            _audioSource.PlayOneShot(soundError);
        }
    }

    public void UpgradeShield()
    {
        if (lvlShield < 4 && SaveSystem.Instante.Save.coins >= lvlShieldPrice)
        {
            lvlShield++;
            lvlShieldValue.text = lvlShield.ToString();
            SaveSystem.Instante.Save.lvlShield = lvlShield;
            SaveSystem.Instante.Save.coins -= lvlShieldPrice;
            ChangePrices();
            GetCoinsValue();
        }
        else
        {
            Debug.Log("errore");
            _audioSource.PlayOneShot(soundError);
        }
    }

    [SerializeField] private TMP_Text explPriceValue;
    [SerializeField] private TMP_Text shieldPriceValue;
    private void ChangePrices()
    {
        bool lvlMaxExpl = false;
        switch (lvlExpl)
        {
            case 1:
                lvlExplPrice = 20; break;
            case 2:
                lvlExplPrice = 40; break;
            case 3: 
                lvlExplPrice = 100; break;
            default:
                lvlMaxExpl = true;
                break;
        }
        explPriceValue.text = lvlMaxExpl ? "MAX" : lvlExplPrice.ToString();

        bool lvlMaxShield = false;
        switch (lvlShield)
        {
            case 1:
                lvlShieldPrice = 40; break;
            case 2:
                lvlShieldPrice = 70; break;
            case 3:
                lvlShieldPrice = 100; break;
            default:
                lvlMaxShield = true;
                break;
        }
        shieldPriceValue.text = lvlMaxShield ? "MAX" : lvlShieldPrice.ToString();
    }

    [SerializeField] private GameObject boxPref;
    [SerializeField] private GameObject crushedBox;
    [SerializeField] private Transform pointBox;
    [SerializeField] private int lootBoxPrice;
    [SerializeField] TMP_Text lootBoxPriceTXT;
    public void OpenLootBox()
    {
        if(SaveSystem.Instante.Save.coins >= lootBoxPrice && _characterMeneger.allCharacterIsOpened == false)
        {
            _audioSource.PlayOneShot(soundUi);
            SaveSystem.Instante.Save.coins -= lootBoxPrice;
            GetCoinsValue();
            StartCoroutine(OpenLootBoxLogic());
        }
        else if(SaveSystem.Instante.Save.countRewordedAd >= 3 && _characterMeneger.allCharacterIsOpened == false)
        {
            StartCoroutine(OpenLootBoxLogic());
        }
        else
        {
            Debug.Log("errore");
            _audioSource.PlayOneShot(soundError);
        }
    }

    [SerializeField] private float speedFollBox = 1;
    private IEnumerator OpenLootBoxLogic()
    {
        eventSystem.SetActive(false);
        int countSwitch = 0;
        if(idChoosedCharacterInMenu == 0)
        {
            Debug.Log(1);
            countSwitch = prefubsInScen.Count;
        }
        else if (idChoosedCharacterInMenu > 0 && idChoosedCharacterInMenu < prefubsInScen.Count - 1)
        {
            countSwitch = prefubsInScen.Count - idChoosedCharacterInMenu;
            Debug.Log(2);
        }
        else if(idChoosedCharacterInMenu == prefubsInScen.Count - 1)
        {
            countSwitch = 1;
            Debug.Log(3);
        }
        
        for (int i = 0; i < countSwitch; i++)
        {
            SwitchCharacters(true);
            yield return new WaitForSeconds(speedMove * 1);
        }

        idChoosedCharacterInMenu++;
        var LootBox = Instantiate(boxPref, pointBox);
        float t = 0;
        while (t < 1)
        {
            LootBox.transform.position = Vector3.Lerp(pointBox.position, choosedCharacterParent.transform.position, t * t);
            t += Time.deltaTime / speedFollBox;
            yield return null;
        }

        Character newCharacter = _characterMeneger.GetRandomClosedCharacterForOpen();

        var newPref = Instantiate(newCharacter.pref, choosedCharacterParent);
        _audioSource.PlayOneShot(soundCharacter);
        prefubsInScen.Add(newPref.transform);
        newPref.transform.GetChild(1).gameObject.SetActive(false);

        var _crushedBox = Instantiate(crushedBox, LootBox.transform.position, Quaternion.identity, pointBox);
        Destroy(LootBox);

        eventSystem.SetActive(true);
        SaveSystem.Instante.DoSaveData();

        yield return new WaitForSeconds(3);
        Destroy(_crushedBox);
        
    }

    [Header("Settings Menu")]
    [SerializeField] private UnityEngine.UI.Slider volumSlider;
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup volum;
    [SerializeField] private TMP_Text languageValue;

    public void ChangeVolume(float value)
    {
        volum.audioMixer.SetFloat("Master", Mathf.Lerp(-80, 0, value));
        SaveSystem.Instante.Save.volume = value;
    }

    public static UnityEngine.Events.UnityEvent onLanguageChange = new UnityEngine.Events.UnityEvent();

    public void ChangeLanguage()
    {
        if(SaveSystem.Instante.Save.language == "en")
        {
            SaveSystem.Instante.Save.language = "ua";
            languageValue.text = "Українська";
        }
        else if(SaveSystem.Instante.Save.language == "ua")
        {
            SaveSystem.Instante.Save.language = "ru";
            languageValue.text = "Русский";
        }
        else if (SaveSystem.Instante.Save.language == "ru")
        {
            SaveSystem.Instante.Save.language = "en";
            languageValue.text = "English";
        }
        onLanguageChange.Invoke();
        SaveSystem.Instante.DoSaveData();
    }

    private void ReadSettingsData()
    {
        volumSlider.value = SaveSystem.Instante.Save.volume;
        if(SaveSystem.Instante.Save.language == "en")
        {
            languageValue.text = "English";
        }
        else if(SaveSystem.Instante.Save.language == "ua")
        {
            languageValue.text = "Українська";
        }
        else if(SaveSystem.Instante.Save.language == "ru")
        {
            languageValue.text = "Русский";
        }
    }

    [Header("Coins value")]
    [SerializeField] private TMP_Text[] coinsValue;

    private void GetCoinsValue()
    {
        for(int i=0; i < coinsValue.Length; i++)
        {
            coinsValue[i].text = SaveSystem.Instante.Save.coins.ToString(); 
        }
    }
    //-------------------------training
    public void OffCursorMain(trainingCursor cursor)
    {
        if(cursor != null)
        {
            cursor.gameObject.SetActive(false);
            SaveSystem.Instante.Save.trainingInMenu = true;
        }
    }
}
