using UnityEngine;

namespace ApplicationFramerate
{
    public class FPS : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_ANDROID
       
            Application.targetFrameRate = 60;
        
            QualitySettings.vSyncCount = 0;
#else
        
        Application.targetFrameRate = -1;

        QualitySettings.vSyncCount = 1;
#endif
        }
    }
}