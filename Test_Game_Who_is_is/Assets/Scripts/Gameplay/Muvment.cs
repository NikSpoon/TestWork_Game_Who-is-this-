using UnityEngine;
public class Muvment : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 0.1f;
    public Vector3 Direction { get; set; }
    private void FixedUpdate()
    {
        Vector3 newDirection = Direction.normalized * speed * Time.fixedDeltaTime;

        _rb.MovePosition(_rb.position + newDirection);

        if (Direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Direction);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, toRotation, rotationSpeed)); 
        }
    }
}
