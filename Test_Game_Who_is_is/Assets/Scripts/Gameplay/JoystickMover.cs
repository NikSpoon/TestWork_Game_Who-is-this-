using System.Collections;
using UnityEngine;

public class JoystickMover : MonoBehaviour
{
    [SerializeField] private VariableJoystick _joystick; 
    private Muvment _movement;
    private void Start()
    {
        _movement = GameObject.FindWithTag("Player").GetComponent<Muvment>();
    }

    private void Update()
    {
        Vector3 input = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);
        _movement.Direction = input;
    }
}