using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _targetCamera;

    void Start()
    {
        _targetCamera = Camera.main;
    }

    void Update()
    {
        if (_targetCamera == null)
        {
            return;
        }

        Vector3 directionToCamera = _targetCamera.transform.position - transform.position;
        directionToCamera.y = 0;

        if (directionToCamera.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);

        transform.rotation = targetRotation;
    }
}