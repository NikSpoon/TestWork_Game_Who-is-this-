using System.Collections.Generic;
using UnityEngine;

public class MemController : MonoBehaviour
{
  
    private Dictionary<GameObject, bool> _playerChoices = new Dictionary<GameObject, bool>();

    public void InitParticipants(List<GameObject> participants)
    {
        _playerChoices.Clear();

        foreach (var player in participants)
        {
            _playerChoices[player] = false; 
        }
    }
    public void SetPlayerChoice(GameObject player, bool isCorrect)
    {
        _playerChoices[player] = isCorrect;
    }


    public bool GetPlayerChoice(GameObject player)
    {
        if (_playerChoices.TryGetValue(player, out bool value))
            return value;

        return false; 
    }

 
    public bool HasPlayerMadeChoice(GameObject player)
    {
        return _playerChoices.ContainsKey(player);
    }

    public void ClearChoices()
    {
        _playerChoices.Clear();
    }
    public int CheckPlayers()
    {
        int i = 0;
        foreach (var pair in _playerChoices.Values)
        {
            if (pair)
            {
                i++;
            }
        }
        return i;
    }
    public void DisableLosers()
    {
        foreach (var pair in _playerChoices)
        {
            if (!pair.Value) 
            {
                pair.Key.SetActive(false); 
            }
        }
    }
}