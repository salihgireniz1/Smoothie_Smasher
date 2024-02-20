namespace Pax
{
    using System;
    using System.Collections;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;

    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        [SerializeField] private List<string> _baseScenesNames;
        private List<string> _levelSceneNames = new List<string>();
        private int _level = 0;

        private void Start()
        {
            //Singelton();
            MainManager.Instance.EventManager.Register(EventTypes.LoadNextScene, LoadNextSceneAsync);
            MainManager.Instance.EventManager.Register(EventTypes.LoadCurrentScene, LoadCurrentSceneAsync);
            InitializeBaseScenesAsync();
        }

        void Singelton()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(gameObject);

            }
        }

        public void InitializeBaseScenesAsync()
        {
            StartCoroutine(InitializeBaseScenesAsyncCoroutine());
        }

        IEnumerator InitializeBaseScenesAsyncCoroutine()
        {
            GetLevelScenes();

            Scene currentScene = SceneManager.GetActiveScene();

            for (int i = 0; i < _baseScenesNames.Count; i++)
            {
                if (currentScene.name != _baseScenesNames[i])
                {
                    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_baseScenesNames[i], LoadSceneMode.Additive);
                    asyncLoad.allowSceneActivation = false;

                    while (asyncLoad.progress < 0.9f)
                    {
                        yield return null;
                    }
                    asyncLoad.allowSceneActivation = true;
                }
            }

            LoadSceneAsync(_levelSceneNames[_level]);
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadSceneAsyncCoroutine(sceneName));
        }

        IEnumerator LoadSceneAsyncCoroutine(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            MainManager.Instance.EventManager.InvokeEvent(EventTypes.LevelLoaded);
            Debug.Log("Level Loaded");

        }

        public void LoadNextSceneAsync(EventArgs args)
        {
            StartCoroutine(LoadNextSceneAsyncCoroutine());
        }

        IEnumerator LoadNextSceneAsyncCoroutine()
        {

            MainManager.Instance.EventManager.InvokeEvent(EventTypes.LoadSceneStart);
            yield return new WaitForSeconds(1.1f);
            UnloadCurrentSceneAsync();

            _level++;

            if (_level < SceneManager.sceneCountInBuildSettings)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_levelSceneNames[_level], LoadSceneMode.Additive);

                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                MainManager.Instance.EventManager.InvokeEvent(EventTypes.LevelLoaded);
                Debug.Log("Next Level Loaded");
            }
            else
            {
                Debug.Log("There is no next scene available.");
            }
        }

        public void LoadPreviousSceneAsync()
        {
            StartCoroutine(LoadPreviousSceneAsyncCoroutine());
        }

        IEnumerator LoadPreviousSceneAsyncCoroutine()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int previousSceneIndex = currentSceneIndex - 1;

            if (previousSceneIndex >= 0)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(previousSceneIndex);

                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
            else
            {
                Debug.Log("There is no previous scene available.");
            }
        }

        public void UnloadCurrentSceneAsync()
        {
            StartCoroutine(UnLoadCurrentSceneAsyncCoroutine());
        }

        IEnumerator UnLoadCurrentSceneAsyncCoroutine()
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(_levelSceneNames[_level]);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        }

        void GetLevelScenes()
        {
            int sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < sceneCountInBuildSettings; i++)
            {
                string sceneName = SceneUtility.GetScenePathByBuildIndex(i);
                sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
                if (!IsBaseScene(sceneName))
                {
                    _levelSceneNames.Add(sceneName);
                    //Debug.Log(sceneName + " Added to list.");
                }

            }
        }

        bool IsBaseScene(string sceneName)
        {
            bool isBaseScene = false;

            for (int i = 0; i < _baseScenesNames.Count; i++)
            {
                if (sceneName == _baseScenesNames[i])
                {
                    isBaseScene = true;
                }
            }

            return isBaseScene;
        }

        void LoadCurrentSceneAsync(EventArgs args)
        {
            StartCoroutine(LoadCurrentSceneAsyncCoroutine());
        }

        IEnumerator LoadCurrentSceneAsyncCoroutine()
        {
            MainManager.Instance.EventManager.InvokeEvent(EventTypes.LoadSceneStart);
            yield return new WaitForSeconds(1.1f);
            UnloadCurrentSceneAsync();

            if (_level < SceneManager.sceneCountInBuildSettings)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_levelSceneNames[_level], LoadSceneMode.Additive);

                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                MainManager.Instance.EventManager.InvokeEvent(EventTypes.LevelLoaded);
                Debug.Log("Current Level Loaded");
            }
            else
            {
                Debug.Log("There is no next scene available.");
            }
        }
    }

}