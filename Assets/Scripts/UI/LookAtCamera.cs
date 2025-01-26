using UnityEngine;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }
        
        private void LateUpdate()
        {
            if (_camera != null)
            {
                transform.LookAt(
                    transform.position + _camera.transform.rotation * Vector3.forward, 
                    _camera.transform.rotation * Vector3.up
                );
            }
        }
    }
}