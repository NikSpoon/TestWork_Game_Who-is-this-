using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemSpawner : MonoBehaviour
{
    [SerializeField] private MemData _memData;
    [SerializeField] private SiginsData _siginsData;

    [SerializeField] private Image _memImageRot; 
    [SerializeField] private Transform _contentRoot;

    [SerializeField] private Transform[] _spawnPoint = new Transform[2];

    private GameObject _siginPrefab;
    private GameObject _truSigin;
    private GameObject _falseSigin;

    private Mem _currentMem;
    private AudioClip _currentMusic;
    public void SpawnMem()
    {
        ClearPrevious();

        if (_memData.mems.Count < 2 || _siginsData.siginPrefabs.Count == 0)
            return;

        _currentMem = _memData.mems[Random.Range(0, _memData.mems.Count)];
        _currentMusic = _currentMem.music;

        var siginData = _siginsData.siginPrefabs[Random.Range(0, _siginsData.siginPrefabs.Count)];
        _siginPrefab = siginData;

        int correctIndex = Random.Range(0, _spawnPoint.Length);
        int incorrectIndex = 1 - correctIndex;

        
        _memImageRot.sprite = _currentMem.sprite;

     
        _truSigin = Instantiate(_siginPrefab, _contentRoot);
        _truSigin.transform.position = _spawnPoint[correctIndex].position;
        _truSigin.GetComponentInChildren<TextMeshProUGUI>().text = _currentMem.name;
        _spawnPoint[correctIndex].GetComponent<Sigin>().isCorrectZone = true;

       
        Mem wrongMem;
        do
        {
            wrongMem = _memData.mems[Random.Range(0, _memData.mems.Count)];
        } while (wrongMem == _currentMem);

        var wrongSiginData = _siginsData.siginPrefabs[Random.Range(0, _siginsData.siginPrefabs.Count)];
        _siginPrefab = wrongSiginData;

        _falseSigin = Instantiate(_siginPrefab, _contentRoot);
        _falseSigin.transform.position = _spawnPoint[incorrectIndex].position;
        _falseSigin.GetComponentInChildren<TextMeshProUGUI>().text = wrongMem.name;
        _spawnPoint[incorrectIndex].GetComponent<Sigin>().isCorrectZone = false;
    }

    public void ClearPrevious()
    {
        foreach (Transform child in _contentRoot)
        {
            Destroy(child.gameObject);
        }

       
        _truSigin = null;
        _falseSigin = null;
    }

    public AudioClip GetMusic()
    {
        return _currentMusic;
    }
}
