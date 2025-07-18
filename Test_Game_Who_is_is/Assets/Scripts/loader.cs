using Assets.Scripts.GameAppControl;
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

        StartCoroutine(UnloadUnusedAssetsRoutine());

    }

    private IEnumerator UnloadUnusedAssetsRoutine()
    {
        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
    public void OnPlay()
    {
        GameTrigger.Instance.SetTrigger(AppTriger.ToMainMenu);
    }
    public void OnExit()
    {
        Application.Quit();
    }
}
