using UnityEngine;


public class AutoRotate : MonoBehaviour
{
    [SerializeField] private bool isOnAwake = true;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private Space rotationType = Space.Self;
    [SerializeField] private Vector3 rotationAxis = Vector3.forward;

    private bool _isRotating;


    private void Awake()
    {
        _isRotating = isOnAwake;
    }


    private void Update()
    {
        if (_isRotating)
        {
            Rotate();   
        }
    }


    public void SetRotation(bool isRotating)
    {
        _isRotating = isRotating;
    }


    private void Rotate()
    {
        var angle = rotationSpeed * Time.deltaTime;

        transform.Rotate(rotationAxis, angle, rotationType);
    }
}
