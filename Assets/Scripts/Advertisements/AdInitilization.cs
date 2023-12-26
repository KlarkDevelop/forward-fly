using UnityEngine;
using UnityEngine.Advertisements;

public class AdInitilization : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androidId;
    [SerializeField] private string iosId;
    [SerializeField] private bool testMod;

    private string gameId;

    private void Awake()
    {
        InitializatAd();
    }

    private void InitializatAd()
    {
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosId : androidId;

        Advertisement.Initialize(gameId, testMod, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ad inttilization completed");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Ad initilization failed. Error: {error} | Message: {message}");
    }
}

