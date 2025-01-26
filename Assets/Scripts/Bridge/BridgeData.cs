using UnityEngine;

namespace Bridge
{
    public class BridgeData : MonoBehaviour
    {
        public bool IsEnabled { get; private set; }

        private void OnEnable()
        {
            IsEnabled = true;
        }

        private void OnDisable()
        {
            IsEnabled = false;
        }
    }
}