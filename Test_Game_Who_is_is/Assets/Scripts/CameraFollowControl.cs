using System.Collections;
using UnityEngine;

public class CameraFollowControl : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -10);
    [SerializeField] private float _smoothSpeed = 2f;
    [SerializeField] private float _rotationSmoothSpeed = 5f;

    private Transform _target;
    private Transform _playerTarget;

    private bool _followActive = false;
    private bool _rotateWithTarget = false;

    private void Start()
    {
        StartCoroutine(InitPlayer());
    }

    private void FixedUpdate()
    {
        if (_rotateWithTarget && _target != null)
        {
            Quaternion targetRotation = Quaternion.Euler(0, _target.eulerAngles.y, 0);
            Vector3 rotatedOffset = targetRotation * _offset;

            Vector3 desiredPosition = _target.position + rotatedOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.fixedDeltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(_target.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _rotationSmoothSpeed * Time.fixedDeltaTime);
        }
        else if (_followActive && _target != null)
        {
            Quaternion lookRotation = Quaternion.LookRotation(_target.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _rotationSmoothSpeed * Time.fixedDeltaTime);

            Vector3 desiredPosition = _target.position + _offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.fixedDeltaTime);
        }
    }

    public void FollowTo(Transform target)
    {
       
        _target = target;
        _followActive = true;
        _rotateWithTarget = false;
    }

    public void FollowToPlayer()
    {
        
        _target = _playerTarget;
        _followActive = true;
        _rotateWithTarget = true;
    }

    public void StopFollowing()
    {
        _followActive = false;
    }

    private IEnumerator InitPlayer()
    {
        GameObject playerObj = null;

        while (playerObj == null)
        {
            playerObj = GameObject.FindWithTag("Player");
            yield return null;
        }

        _playerTarget = playerObj.transform;
        _target = _playerTarget;

        transform.position = _target.position + _offset;

        
        _followActive = true;
        _rotateWithTarget = true;
    }
}
