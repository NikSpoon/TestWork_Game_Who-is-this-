using UnityEngine;

public class Sigin : MonoBehaviour
{
    [SerializeField] private MemController _memController;
    [SerializeField] private Material _originalMaterial;
    [SerializeField] private Material _highlightMaterial;
    [SerializeField] private LayerMask _playerLayer;

    private Renderer _renderer;
    public bool isCorrectZone { get; set; } = false;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer != null && _originalMaterial != null)
        {
            _renderer.material = _originalMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer) != 0)
        {
            if (other.CompareTag("Player") && _renderer != null && _highlightMaterial != null)
            {
                _renderer.material = _highlightMaterial;
            }

            _memController?.SetPlayerChoice(other.gameObject, isCorrectZone);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer) != 0)
        {
            if (other.CompareTag("Player") && _renderer != null && _highlightMaterial != null)
            {
                _renderer.material = _originalMaterial;
            }
            _memController?.SetPlayerChoice(other.gameObject, false);
        }
    }
}
