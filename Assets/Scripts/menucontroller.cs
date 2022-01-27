using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class menucontroller : MonoBehaviour
{
    public void Ad()
    {
        //ads.instance.RequestAndLoadRewardedAd();
        if (ads.instance == null)
            Debug.Log("no instance");
        ads.instance.ShowRewardedAd();
    }
    public void ReqAd()
    {
        if (ads.instance == null)
            Debug.Log("no instance");
        ads.instance.RequestAndLoadRewardedAd();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
