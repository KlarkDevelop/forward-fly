using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static InterstitialAd Instante { get; private set; }

    [SerializeField] private string androidAdId = "Interstitial_Android";
    [SerializeField] private string iosAdId = "Interstitial_iOS";
    private string adId;

    private void Awake()
    {
        Instante = this;
        adId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosAdId : androidAdId;
    }

    private void Start()
    {
        LoadAd();
    }

    public void ShowAd()
    {
        Debug.Log("Showing interstitial ad. Id: " + adId);
        Advertisement.Show(adId, this);
    }

    private void LoadAd()
    {
        Debug.Log("Start load instertitial ad");
        Advertisement.Load(adId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial ad loaded. Placement Id: " + placementId);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Interstitial ad failed load. Placement Id: {placementId} |\n Errore: {error} |\n Message: {message}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"Interstitial ad show complete. Placement Id: {placementId} | State: {showCompletionState}");
        SaveSystem.Instante.Save.countDeath = 0;
        LoadAd();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Interstitial ad show failed. Placement Id: {placementId} | \nError: {error} |\nMessage: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Interstitial ad start show. Placement Id: " + placementId);
    }
}
