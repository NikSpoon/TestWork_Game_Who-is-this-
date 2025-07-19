using FSM;
using System;

public interface IAppSystem
{
    AppState CurrentState { get; }
    void Trigger(AppTriger trigger);
    event Action<StateChangeData<AppState, AppTriger>> OnStateChange;
}

public class AppSystem : IAppSystem
{
    private StateMashine<AppState, AppTriger> _stateMashine;
    public AppState CurrentState => _stateMashine.CurrentState;

    event Action<StateChangeData<AppState, AppTriger>> IAppSystem.OnStateChange
    {
        add => _stateMashine.OnStateChange += value;
        remove => _stateMashine.OnStateChange -= value;
    }

    public AppSystem()
    {
        _stateMashine = new StateMashine<AppState, AppTriger>(AppState.Loading);

        _stateMashine.AddTransition(AppState.Loading, AppTriger.ToMainMenu, AppState.MainMenu);

        _stateMashine.AddTransition(AppState.MainMenu, AppTriger.ToGameplay, AppState.Gameplay);

        _stateMashine.AddTransition(AppState.Gameplay, AppTriger.ToMainMenu, AppState.MainMenu);
       

    }

    public void Trigger(AppTriger trigger)
    {
        _stateMashine.SetTrigger(trigger);
    }

}

public enum AppState
{
    Loading,
    MainMenu,
    Gameplay,
  

}

public enum AppTriger
{
    ToMainMenu,
    ToGameplay,
 
}



