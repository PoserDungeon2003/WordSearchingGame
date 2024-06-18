using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{
    public string appId;
    public string adBannerId;
    public string adInterstitialId;
    public string adBannerIdTestDevice;
    public string adInterstitialIdTestDevice;
    public AdPosition bannerPosition;
    public bool testDevice = false;

    private BannerView _bannerView;
    private InterstitialAd _interstitial;

    public static AdManager Instance;

    public static Action OnInterstitialAdsClosed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        var adRequest = new AdRequest();
        if (testDevice)
        {
            RequestBanner();
            adInterstitialId = adInterstitialIdTestDevice;
            adBannerId = adBannerIdTestDevice;
            RequestConfiguration requestConfiguration = new RequestConfiguration();
            requestConfiguration.TestDeviceIds.Add("217ED492E4571501ED78FCA4E7818388");
            MobileAds.SetRequestConfiguration(requestConfiguration);
        }
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            CreateBanner(adRequest);
            LoadInterstitialAd(adRequest);
        });

        _interstitial.OnAdFullScreenContentClosed += OnInterstitialAdsClosed;
    }

    private void OnDisable()
    {
        if (_interstitial != null)
            _interstitial.OnAdFullScreenContentClosed -= OnInterstitialAdsClosed;
    }

    private void InterstitialAdClosed(object sender, EventArgs e)
    {
        if (OnInterstitialAdsClosed != null)
        {
            OnInterstitialAdsClosed();
        }
    }

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd(AdRequest adRequest)
    {
        if (_interstitial != null)
        {
            _interstitial.Destroy();
            _interstitial = null;
        }
        Debug.Log("Loading the interstitial ad.");

        InterstitialAd.Load(adInterstitialId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                Debug.LogError("interstitial ad failed to load an ad " +
                               "with error : " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response : "
                      + ad.GetResponseInfo());

            _interstitial = ad;
            RegisterEventHandlers(_interstitial);
        });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (_interstitial != null && _interstitial.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitial.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    public void CreateBanner(AdRequest request)
    {
        this._bannerView = new BannerView(adBannerId, AdSize.SmartBanner, bannerPosition);
        this._bannerView.LoadAd(request);
        HideBanner();
    }

    public void HideBanner() => _bannerView.Hide();

    public void ShowBanner() => _bannerView.Show();

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            var adRequest = new AdRequest();
            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd(adRequest);
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            var adRequest = new AdRequest();
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd(adRequest);
        };
    }

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        var bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        // Create an empty ad request.
        AdRequest request = new AdRequest();
        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

}
