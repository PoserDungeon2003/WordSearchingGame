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
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            //CreateBanner(adRequest);
            LoadInterstitialAd(adRequest);
        });

        this._interstitial.OnAdFullScreenContentClosed += OnInterstitialAdsClosed;
    }

    private void OnDisable()
    {
        this._interstitial.OnAdFullScreenContentClosed -= OnInterstitialAdsClosed;
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
        if (this._interstitial != null)
        {
            this._interstitial.Destroy();
            this._interstitial = null;
        }
        Debug.Log("Loading the interstitial ad.");

        InterstitialAd.Load(appId, adRequest, (InterstitialAd ad, LoadAdError error) =>
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

            this._interstitial = ad;
            RegisterEventHandlers(_interstitial);
        });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (this._interstitial != null && this._interstitial.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            this._interstitial.Show();
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
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

}
