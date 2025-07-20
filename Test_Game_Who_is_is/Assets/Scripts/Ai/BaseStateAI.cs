using UnityEngine;
public interface IState
{
    void OnEnter();
    void OnExit();
    void OnUpdate();
}
public abstract class BaseStateAI : MonoBehaviour, IState
{
    [SerializeField] protected Muvment _muvment;
    public abstract void OnEnter();

    public abstract void OnExit();
    public abstract void OnUpdate();
    
    private void Update()
    {
        OnUpdate();
    }
   
}
