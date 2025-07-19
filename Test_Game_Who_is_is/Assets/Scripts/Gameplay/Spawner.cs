using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> _spawnPoints;

    [Header("Other")]
    [SerializeField] private Transform _banner;

    private int _playerIndex;
   
    private void Start()
    {
        if (_spawnPoints == null || _spawnPoints.Count == 0 || _banner == null)  return;
        
        SpawnPlayer();
        SpawnOther(_spawnPoints.Count - 1);
    }

    private void SpawnPlayer()
    {
        if (_spawnPoints.Count == 0) return;
      
        System.Random random = new System.Random(); 
        int playerIndex = random.Next(0, _spawnPoints.Count);

        Transform playerSpawnPoint = _spawnPoints[playerIndex];
        _playerIndex = playerIndex;

        Quaternion lookRotation = Quaternion.LookRotation(_banner.position - playerSpawnPoint.position);

        Instantiate(_player, playerSpawnPoint.position, lookRotation);
    }

    private void SpawnOther(int enemyCount)
    {
        int spawned = 0;
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (i == _playerIndex)  continue;

            if (spawned >= enemyCount) break;

            Transform spawnPoint = _spawnPoints[i];
            Quaternion lookRotation = Quaternion.LookRotation(_banner.position - spawnPoint.position);

            Instantiate(_enemy, spawnPoint.position, lookRotation);
            spawned++;
        }
    }
}
