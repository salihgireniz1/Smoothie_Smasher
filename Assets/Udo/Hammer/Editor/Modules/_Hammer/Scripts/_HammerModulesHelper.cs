using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Udo.Hammer.Editor.Backend.UI;
using Udo.Hammer.Runtime.Core;
using UnityEditor;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace Udo.Hammer.Editor.Modules._Hammer
{
    // ReSharper disable once InconsistentNaming
    public static class _HammerModulesHelper
    {
        private static readonly string[] PathsEditorHammerConfigObject =
            { "Assets", "Udo", "Hammer", "Editor", "Backend", "Scripts", "UI", "_HammerConfig.asset" };

        private static readonly string[] PathsRuntimeHammerConfigObject =
            { "Assets", "Udo", "Hammer", "Runtime", "Core", "HammerConfig.asset" };

        private static readonly string[] PathsZipOutputFolder = { Application.dataPath, "..", "Hammer" };

        public static _HammerEditorWindowConfigObject GetEditorConfig()
        {
            var hammerEditorWindowConfigObject =
                AssetDatabase.LoadAssetAtPath<_HammerEditorWindowConfigObject>(
                    Path.Combine(PathsEditorHammerConfigObject));
            return hammerEditorWindowConfigObject;
        }

        public static bool ShouldUpdateRuntime()
        {
            var hammerEditorWindowConfigObject = GetEditorConfig();
            return hammerEditorWindowConfigObject != null && hammerEditorWindowConfigObject.shouldUpdateRuntime;
        }

        public static void CreateLibrary(string zipName, List<_HammerFileOrFolderDetails> fileOrFolderDetailsList)
        {
            CreateZip(zipName);
            var entries = new HashSet<string>();
            foreach (var path in fileOrFolderDetailsList)
            {
                try
                {
                    entries = AddFilesToZip(zipName, path.Folders, path.FileOrFolder, entries, path.IsFile);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private static void CreateZip(string zipName)
        {
            if (!ShouldCreateZip()) return;

            CreateModulesFolder();

            var zipPath = Path.Combine(Path.Combine(PathsZipOutputFolder), zipName + ".zip");
            if (File.Exists(zipPath)) File.Delete(zipPath);

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                }

                using (var fileStream = new FileStream(zipPath, FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
            }
        }

        private static bool ShouldCreateZip()
        {
            var hammerEditorWindowConfigObject =
                AssetDatabase.LoadAssetAtPath<_HammerEditorWindowConfigObject>(
                    Path.Combine(PathsEditorHammerConfigObject));
            return hammerEditorWindowConfigObject != null && hammerEditorWindowConfigObject.createZips;
        }

        public static void CreateModulesFolder()
        {
            var rootFolder = Path.GetDirectoryName(Application.dataPath);
            var hammerFolder = Path.Combine(rootFolder, "Hammer");
            if (!Directory.Exists(hammerFolder))
            {
                Directory.CreateDirectory(hammerFolder);
            }
            var gitignoreFile = Path.Combine(hammerFolder, ".gitignore");
            if (!File.Exists(gitignoreFile))
            {
                File.WriteAllText(gitignoreFile, "*\n!.gitignore");
            }
            var tmpFolder = Path.Combine(hammerFolder, "tmp");
            if (!Directory.Exists(tmpFolder))
            {
                Directory.CreateDirectory(tmpFolder);
            }
            else
            {
                Directory.Delete(tmpFolder, true);
                Directory.CreateDirectory(tmpFolder);
            }
        }

        private static HashSet<string> AddFilesToZip(string zipName, string[] folders, string fileOrFolder, HashSet<string> entries, bool isFile = false)
        {
            if (!ShouldCreateZip()) return entries;

            var result = new HashSet<string>(entries);
            var zipPath = Path.Combine(Path.Combine(PathsZipOutputFolder), zipName + ".zip");

            using (var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Update))
            {
                var folderPath = "";

                for (var i = 0; i < folders.Length; i++)
                {
                    var paths = new List<string>();
                    for (var j = 0; j < i; j++) paths.Add(folders[j]);

                    var folderName = folders[i];
                    folderPath = Path.Combine(Path.Combine(paths.ToArray()), folderName);
                    folderPath = folderPath.Replace("\\", "/");
                    var folderEntryName = folderPath + "/";
                    if (!result.Contains(folderEntryName))
                    {
                        zipArchive.CreateEntry(folderEntryName, CompressionLevel.Optimal);
                        result.Add(folderEntryName);
                    }

                    var folderMetaFileEntryName = folderPath + ".meta";
                    var folderMetaFilePath = Path.Combine(Application.dataPath, folderMetaFileEntryName);
                    folderMetaFilePath = folderMetaFilePath.Replace("\\", "/");
                    if (!result.Contains(folderMetaFileEntryName))
                    {
                        zipArchive.CreateEntryFromFile(folderMetaFilePath, folderMetaFileEntryName, CompressionLevel.Optimal);
                        result.Add(folderMetaFileEntryName);
                    }
                }

                var entryName = fileOrFolder;
                if (!string.IsNullOrEmpty(folderPath)) entryName = Path.Combine(folderPath, entryName);
                entryName = entryName.Replace("\\", "/");
                var entryMetaFileName = entryName + ".meta";
                var entryMetaFilePath = Path.Combine(Application.dataPath, entryMetaFileName);
                entryMetaFilePath = entryMetaFilePath.Replace("\\", "/");
                if (!result.Contains(entryMetaFileName))
                {
                    zipArchive.CreateEntryFromFile(entryMetaFilePath, entryMetaFileName, CompressionLevel.Optimal);
                    result.Add(entryMetaFileName);
                }
                var entryPath = Path.Combine(Application.dataPath, entryName);
                entryPath = entryPath.Replace("\\", "/");
                if (isFile)
                {
                    if (!result.Contains(entryName))
                    {
                        zipArchive.CreateEntryFromFile(entryPath, entryName, CompressionLevel.Optimal);
                        result.Add(entryName);
                    }
                }
                else
                {
                    entryName += "/";
                    if (!result.Contains(entryName))
                    {
                        var entriesOfDirectory = zipArchive.CreateEntryFromDirectory(entryPath, entryName);
                        result.Add(entryName);
                        foreach (var entry in entriesOfDirectory)
                        {
                            result.Add(entry);                        
                        }
                    }
                }
            }

            return result;
        }

        private static HashSet<string> CreateEntryFromDirectory(this ZipArchive archive, string sourceDirName,
            string entryName = "", CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            var result = new HashSet<string>();
            var folders = new Stack<string>();
            folders.Push(sourceDirName);

            do
            {
                var currentFolder = folders.Pop();
                var files = Directory.GetFiles(currentFolder).ToList();
                files.ForEach(f =>
                {
                    var entry = Path.Combine(entryName, f.Substring(sourceDirName.Length + 1));
                    entry = entry.Replace("\\", "/");
                    if (!result.Contains(entry))
                    {
                        archive.CreateEntryFromFile(f, entry, compressionLevel);
                        result.Add(entry);                        
                    }
                });
                var directories = Directory.GetDirectories(currentFolder).ToList();
                directories.ForEach(d => folders.Push(d));
            } while (folders.Count > 0);
            return result;
        }

        public static HammerConfigObject GetRuntimeConfig()
        {
            var hammerConfigObject =
                AssetDatabase.LoadAssetAtPath<HammerConfigObject>(Path.Combine(PathsRuntimeHammerConfigObject));
            return hammerConfigObject;
        }

        public static void CreateFile(string path, string text)
        {
            File.WriteAllText(path, text);
        }

        public static void UninstallSdk(List<_HammerFileOrFolderDetails> fileOrFolderDetailsList)
        {
            foreach (var p in fileOrFolderDetailsList)
            {
                try
                {
                    var relativePath = Path.Combine(Path.Combine(p.Folders), p.FileOrFolder);
                    var relativePathAssets = Path.Combine("Assets", relativePath);
                    AssetDatabase.DeleteAsset(relativePathAssets);
                    if (p.Folders.Length > 0)
                    {
                        for (var i = p.Folders.Length; i > 0; i--)
                        {
                            var paths = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                paths[j] = p.Folders[j];
                            }

                            DeleteDirectoryIfEmpty(paths);
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            AssetDatabase.Refresh();
        }

        private static void DeleteDirectoryIfEmpty(string[] path)
        {
            try
            {
                var absolutePath = Path.Combine(Application.dataPath, Path.Combine(path));
                var directoryInfo = new DirectoryInfo(absolutePath);
                var relativePath = Path.Combine("Assets", Path.Combine(path));
                var filesInSubDirectory = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                if (filesInSubDirectory.Length == 0 ||
                    !filesInSubDirectory.Any(t => t.FullName.EndsWith(".meta") == false))
                    AssetDatabase.DeleteAsset(relativePath);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}