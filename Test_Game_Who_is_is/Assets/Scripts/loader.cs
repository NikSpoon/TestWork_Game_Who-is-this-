
using System.Collections;
using UnityEngine;

public class loader : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;


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
