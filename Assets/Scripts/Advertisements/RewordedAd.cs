using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewordedAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static RewordedAd Instante { get; private set; }

    [SerializeField] private string androidAdId = "Rewarded_Android";
    [SerializeField] private string iosAdId = "Rewarded_iOS";
    private string adId;

    private void Awake()
    {
        Instante = this;
        adId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosAdId : androidAdId;
    }

    private void Start()
    {
        StartCoroutine(LoadAdAfterTime());

        if(adCounter != null)
        {
            if (SaveSystem.Instante.IsNewDayFromLastAdLootBox())
            {
                adCounter.text = "X " + (3 - SaveSystem.Instante.Save.countRewordedAd).ToString();
            }
            else
            {
                adCounter.text = "X 0";
            }
        }
    }

    private IEnumerator LoadAdAfterTime()
    {
        yield return new WaitForSeconds(1);
        LoadAd();
    }
    private void LoadAd()
    {
        Debug.Log("Start load reworded ad");
        if( (type == "continue" && oneContinue == true) || (type == "lootBox" && _characterMeneger.allCharacterIsOpened == false && SaveSystem.Instante.IsNewDayFromLastAdLootBox()) )
        {
            Advertisement.Load(adId, this);
        }
        else
        {
            bool ChekTime = SaveSystem.Instante.IsNewDayFromLastAdLootBox();
            string charactersInfo = (_characterMeneger != null) ? _characterMeneger.allCharacterIsOpened.ToString() : "CharacterMeneger is null";
            Debug.Log($"Reworded ad is not loaded | Type: {type} | Continue: {oneContinue} | AllCharactersOpened: { charactersInfo } | Chek date: {ChekTime}");
        }
    }

    [Header("Settings for show")]
    [SerializeField] private TMPro.TMP_Text adCounter;
    [SerializeField] private MainMenuMeneger _mainMenuMeneger;
    [SerializeField] private UnityEngine.UI.Button buttonAd;
    [SerializeField] private CharacterMeneger _characterMeneger;

    private bool oneContinue = true;
    [SerializeField]private string type; //"lootBox", "continue"
    public void ShowAd()
    {
        buttonAd.interactable = false;
        Debug.Log("Showing reworded ad. Id: " + adId);
        Advertisement.Show(adId, this);
    }

    public static UnityEngine.Events.UnityEvent onContinueAdComplet = new UnityEngine.Events.UnityEvent();
    private void GiveReword()
    {
        Debug.Log("Give reword type: " + type);

        if(type == "lootBox")
        {
            SaveSystem.Instante.Save.countRewordedAd += 1;
            adCounter.text = "X " + (3 - SaveSystem.Instante.Save.countRewordedAd).ToString();
            if (SaveSystem.Instante.Save.countRewordedAd >= 3)
            {
                _mainMenuMeneger.OpenLootBox();
                SaveSystem.Instante.Save.dayGetCharacterFromAdBox = DateTime.Today.Day;
                SaveSystem.Instante.Save.countRewordedAd = 0;
            }
        }
        else if (type == "continue")
        {
            oneContinue = false;
            onContinueAdComplet.Invoke();
        }
        else
        {
            Debug.Log($"Type {type} does not exist");
        }
    }
    [SerializeField] private Canvas ErrorWind;
    private void ShowErrorWindow()
    {
        Debug.Log("Show error window");
        ErrorWind.gameObject.SetActive(true);
    }

    #region Listeners

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Reworded ad loaded. Placement Id: " + placementId);
        buttonAd.interactable = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Reworded ad failed load. Placement Id: {placementId} |\n Errore: {error} |\n Message: {message}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Reworded ad show complete. Placement Id: {placementId} | State: {showCompletionState}");

        if(placementId.Equals(adId) && showCompletionState.ToString() == UnityAdsCompletionState.COMPLETED.ToString())
        {
            GiveReword();
        }
        else
        {
            ShowErrorWindow();
        }
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Reworded ad show failed. Placement Id: {placementId} | \nError: {error} |\nMessage: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Reworded ad start show. Placement Id: " + placementId);
    }
    #endregion
}
