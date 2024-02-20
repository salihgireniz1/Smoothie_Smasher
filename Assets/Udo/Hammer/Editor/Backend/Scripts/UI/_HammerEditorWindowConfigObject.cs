using System;
using UnityEditor;
using UnityEngine;

namespace Udo.Hammer.Editor.Backend.UI
{
    // ReSharper disable once InconsistentNaming
    [CreateAssetMenu(fileName = "_HammerConfig", menuName = "UDO/Hammer/Editor Config", order = 0)]
    public class _HammerEditorWindowConfigObject : ScriptableObject
    {
        public string email;
        public string password;
        public string gameId;
        public bool createZips;
        public bool shouldUpdateRuntime;
        [SerializeField] private string timeSyncFileString;
        [SerializeField] private string timeSyncConfigString;
        private DateTime _timeSyncConfig;

        private DateTime _timeSyncFile;

        private void OnValidate()
        {
            if (createZips) EditorUtility.RequestScriptReload();
        }

        public void SetTimeSyncFile(DateTime dateTime)
        {
            _timeSyncFile = dateTime;
            timeSyncFileString = _timeSyncFile.ToLongDateString() + " " + _timeSyncFile.ToLongTimeString();
            EditorUtility.SetDirty(this);
        }

        public void SetTimeSyncConfig(DateTime dateTime)
        {
            _timeSyncConfig = dateTime;
            timeSyncConfigString = _timeSyncConfig.ToLongDateString() + " " + _timeSyncConfig.ToLongTimeString();
            EditorUtility.SetDirty(this);
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            gameId = null;
            SetTimeSyncFile(new DateTime());
            SetTimeSyncConfig(new DateTime());

            EditorUtility.SetDirty(this);
            AssetDatabase.Refresh();
            EditorUtility.RequestScriptReload();
        }
    }
}