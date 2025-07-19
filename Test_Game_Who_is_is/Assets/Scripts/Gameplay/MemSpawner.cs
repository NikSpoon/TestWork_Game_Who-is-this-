using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemSpawner : MonoBehaviour
{
    [SerializeField] private MemData _memData;
    [SerializeField] private SiginsData _siginsData;
    
    [SerializeField] private RectTransform _root;

    [SerializeField] private GameObject _imagePrefab;

    [SerializeField] private Transform[] _spawnPoint = new Transform[2];

   
    private GameObject _siginPrefab;
    private Image _memImage;
    private GameObject _truSigin;
    private GameObject _falseSigin;

    private Mem _currentMem;

    public void SpawnMem()
    {
        ClearPrevious();

        if (_memData.mems.Count < 2 || _siginsData.siginPrefabs.Count == 0)
            return;

        _currentMem = _memData.mems[Random.Range(0, _memData.mems.Count)];

        var siginData = _siginsData.siginPrefabs[Random.Range(0, _siginsData.siginPrefabs.Count)];
        _siginPrefab = siginData;


        int correctIndex = Random.Range(0, _spawnPoint.Length);
        int incorrectIndex = 1 - correctIndex;

        GameObject imageGO = Instantiate(_imagePrefab, _root);
        _memImage = imageGO.GetComponent<Image>();
        _memImage.sprite = _currentMem.sprite;

        _truSigin = Instantiate(_siginPrefab, _spawnPoint[correctIndex].position, Quaternion.identity, _root);
        _truSigin.GetComponentInChildren<TextMeshProUGUI>().text = _currentMem.name;
        _spawnPoint[correctIndex].gameObject.GetComponent<Sigin>().isCorrectZone = true;


        Mem wrongMem;
        do
        {
            wrongMem = _memData.mems[Random.Range(0, _memData.mems.Count)];
        } while (wrongMem == _currentMem);
       
        var wronSiginData = _siginsData.siginPrefabs[Random.Range(0, _siginsData.siginPrefabs.Count)];
        _siginPrefab = wronSiginData;

        _falseSigin = Instantiate(_siginPrefab, _spawnPoint[incorrectIndex].position, Quaternion.identity, _root);
        _falseSigin.GetComponentInChildren<TextMeshProUGUI>().text = wrongMem.name;
        _spawnPoint[correctIndex].gameObject.GetComponent<Sigin>().isCorrectZone = true;
    }

    private void ClearPrevious()
    {
        if (_memImage != null)
            Destroy(_memImage.gameObject);

        if (_truSigin != null)
            Destroy(_truSigin);

        if (_falseSigin != null)
            Destroy(_falseSigin);
    }
}
