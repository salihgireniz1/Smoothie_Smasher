using UnityEngine;
using UnityEngine.SceneManagement;

namespace Udo.Hammer.Runtime.Core
{
    [DefaultExecutionOrder(-1)]
    public class HammerInitializer : MonoBehaviour
    {
        public HammerConfigObject hammerConfigObject;
        private Hammer _instance;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Hammer.HammerConfigObject = hammerConfigObject;
            _instance = Hammer.Instance;
            Debug.Log(_instance.gameObject.name);
        }

        private void Start()
        {
            _instance.onSdkInitializationEnd.AddListener(OnSdkInitializationEnd);
            var hammerTransform = _instance.gameObject.transform;
            while (transform.childCount > 0)
            {
                var child = transform.GetChild(0);
                child.parent = hammerTransform;
            }
        }

        private void OnSdkInitializationEnd()
        {
            Debug.Log(gameObject.name + "OnSdkInitializationEnd");
            SceneManager.LoadSceneAsync(1);
        }
    }
}