using UnityEngine;

public class AnimPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private float speedThreshold = 0.1f;

    void Update()
    {
        if (_rb == null || _animator == null)
            return;

        float speed = _rb.linearVelocity.magnitude;
        bool isMoving = speed > speedThreshold;

        _animator.SetBool("isWalking", isMoving);
    }
}