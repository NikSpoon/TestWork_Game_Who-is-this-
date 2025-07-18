using UnityEngine;
public interface IStateLogic
{
    void Enter();
    void Exit();
    void AppUpdate();
    bool IsReady { get; }
}
public abstract class BaseGameState : MonoBehaviour, IStateLogic
{
    protected GameManager Manager { get; private set; }
    public abstract bool IsReady { get; }
    
    public void Init(GameManager manager)
    {
        Manager = manager;
   
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void AppUpdate();


}
