using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public static AdManager Instance;

    [SerializeField] private bool testMode = true;

    private GameOverHandler gameOverHandler;

#if UNITY_ANDROID
    private string gameId = "4514035";
#elif UNITY_IOS
    private string gameId = "4514034";
#endif

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
        }
    }

#region Ads

    public void ShowAd(GameOverHandler gameOverHandler)
    {
        this.gameOverHandler = gameOverHandler;
        Advertisement.Show("rewardedVideo");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError($"Unity Ads Error: {message}");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                Debug.LogWarning("Ad failed");
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad skipped");
                break;
            case ShowResult.Finished:
                gameOverHandler.ContinueGame();
                break;
            default:
                break;
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ad Started");
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Unity Ads Ready");
    }

#endregion
}
