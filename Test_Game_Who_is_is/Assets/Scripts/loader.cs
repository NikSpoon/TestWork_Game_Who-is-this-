
using System.Collections;
using UnityEngine;

public class loader : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

        QualitySettings.shadows = ShadowQuality.HardOnly;
        QualitySettings.shadowDistance = 15f;
        QualitySettings.lodBias = 0.5f;

        Time.fixedDeltaTime = 0.03f; 

        OnPlay();
    }

    public void OnPlay()
    {
       GameManager.Instance.Trigger(AppTriger.ToMainMenu);
    }
    public void OnExit()
    {
        Application.Quit();
    }
}
