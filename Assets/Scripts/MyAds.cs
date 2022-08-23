using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum AdsType { video, rewardedVideo}

public class MyAds : MonoBehaviour//, IPointerDownHandler
{
	public AdsType currentAdType;
	string adType;
	string gameID = "3673713";

	Button continueButton;
	
    // Start is called before the first frame update
    void Start()
    {
		Advertisement.Initialize(gameID, true);
    }

	private void Update()
	{
		adType = currentAdType.ToString();

		//continueButton.onClick.AddListener(currentAdType = AdsType.rewardedVideo);
		//if(continueButton.)
	}

	public void ShowAds()
	{

		if (Advertisement.IsReady())
		{
			Advertisement.Show(adType);
		}
	}

	/*public void OnPointerDown(PointerEventData eventData)
	{
		currentAdType = AdsType.rewardedVideo;
		continueButton.onClick.AddListener(ShowAds);
	}*/
}
