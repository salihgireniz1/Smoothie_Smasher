using Udo.Hammer.Runtime.Core;
using UnityEngine;

public class HammerDemoScript : MonoBehaviour
{
    public void OnGUI()
    {
        GUI.backgroundColor = Color.blue;
        GUI.skin.button.fontSize = (int)(0.035f * Screen.width);

        var showRewardedVideoButton = new Rect(0.10f * Screen.width, 0.15f * Screen.height, 0.80f * Screen.width,
            0.08f * Screen.height);
        if (GUI.Button(showRewardedVideoButton, "Show Rewarded"))
        {
            Debug.Log("unity-script: ShowRewardedVideoButtonClicked");

            Hammer.Instance.MEDIATION_HasRewarded(
                () =>
                {
                    Hammer.Instance.MEDIATION_ShowRewarded(
                        () => { Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - True"); },
                        s => { Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False - " + s); });
                },
                s => { Debug.Log("unity-script: IronSource.Agent.isRewardedVideoAvailable - False - " + s); });
        }

        var showInterstitialVideoButton = new Rect(0.10f * Screen.width, 0.25f * Screen.height, 0.80f * Screen.width,
            0.08f * Screen.height);
        if (GUI.Button(showInterstitialVideoButton, "Show Interstitial"))
        {
            Debug.Log("unity-script: showInterstitialVideoButtonClicked");

            Hammer.Instance.MEDIATION_HasInterstitial(
                () =>
                {
                    Hammer.Instance.MEDIATION_ShowInterstitial(
                        () => { Debug.Log("unity-script: IronSource.Agent.isInterstitialVideoAvailable - True"); },
                        s =>
                        {
                            Debug.Log("unity-script: IronSource.Agent.isInterstitialVideoAvailable - False - " + s);
                        });
                },
                s => { Debug.Log("unity-script: IronSource.Agent.isInterstitialVideoAvailable - False - " + s); });
        }

        var loadBannerButton = new Rect(0.10f * Screen.width, 0.35f * Screen.height, 0.35f * Screen.width,
            0.08f * Screen.height);
        if (GUI.Button(loadBannerButton, "Load Banner"))
        {
            Debug.Log("unity-script: loadBannerButtonClicked");
            Hammer.Instance.MEDIATION_ShowBanner();
        }

        var destroyBannerButton = new Rect(0.55f * Screen.width, 0.35f * Screen.height, 0.35f * Screen.width,
            0.08f * Screen.height);
        if (GUI.Button(destroyBannerButton, "Destroy Banner"))
        {
            Debug.Log("unity-script: loadBannerButtonClicked");
            Hammer.Instance.MEDIATION_DestroyBanner();
        }
    }
}