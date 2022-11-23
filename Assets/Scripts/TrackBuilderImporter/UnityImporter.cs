using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine.Windows;
using Object = UnityEngine.Object;

namespace TrackBuilder
{
    public class UnityImporter : MonoBehaviour
    {
        [SerializeField] private string importFilePath;

        // GridSettings
        public int gridHeight;
        public int gridWidth ;
        public int gridLength;
        public string assetBundlePath;

        private Dictionary<Vector3, GameObject> gridTiles = new Dictionary<Vector3, GameObject>();
        [HideInInspector] public static object[] assets;
        List<ICommand> commandList = new List<ICommand>();

        [ContextMenu("Import Track")]
        public void Import()
        {
            Debug.Log("Importing");

            BinaryFormatter BFormatter = new BinaryFormatter();
            FileStream fs = null;
            List<Data> dataList = new List<Data>();
            Settings settings = new Settings();
            GridBuilderProjectData importData = new GridBuilderProjectData();

            try
            {
                fs = new FileStream(importFilePath, FileMode.Open);

                importData = (GridBuilderProjectData)BFormatter.Deserialize(fs);
                dataList = importData.data;
                settings = importData.settings;

                // Import gridsettings
                gridHeight = settings.gridHeight;
                gridWidth = settings.gridWidth;
                gridLength = settings.gridLength;
                assetBundlePath = settings.assetBundlePath;

                // Generate grid
                for (int x = 0; x < gridWidth; x++)
                {
                    for (int y = 0; y < gridHeight; y++)
                    {
                        for (int z = 0; z < gridLength; z++)
                        {
                            gridTiles.Add(new Vector3Int(x, y, z), null);
                        }
                    }
                }

                // Get prefabs from assetpack
                AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, assetBundlePath));
                Object[] bundleAssets = bundle.LoadAllAssets();
                assets = bundleAssets;

                // Import gridobjects
                foreach (Data data in dataList)
                {
                    ICommand command = null;

                    // Select Command Type
                    if (data.isErase)
                    {
                        command = CreateCommand(typeof(EraseObjectCommand), data);
                    }
                    else
                    {
                        command = CreateCommand(typeof(PlaceObjectCommand), data);
                    }

                    commandList.Add(command);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Serialization Error: " + e.Message);
            }

            foreach (ICommand command in commandList)
            {
                gridTiles = command.Execute(gridTiles);
            }
        }

        private static ICommand CreateCommand(Type _classType, Data _data)
        {
            // Get Prefab
            GameObject prefab = null;
            foreach (Object asset in assets)
            {
                GameObject tempPrefab = (GameObject)asset;
                if (tempPrefab.name == _data.PrefabName)
                {
                    // Load prefab
                    prefab = (GameObject)asset;
                }
            }

            // Create Command
            ICommand command = (ICommand)Activator.CreateInstance(
                _classType,
                prefab,
                new Vector3(
                    _data.positionX,
                    _data.positionY,
                    _data.positionZ),
                new Quaternion(
                    _data.rotationX,
                    _data.rotationY,
                    _data.rotationZ,
                    _data.rotationW)
            );

            return command;
        }
        //    [SerializeField] private TextAsset importFile;

        //    [ContextMenu("Import Track")]
        //    public void Import()
        //    {
        //        Debug.Log("Importing");

        //        string url = Path.Combine(AssetDatabase.GetAssetPath(importFile));

        //        BinaryFormatter BFormatter = new BinaryFormatter();
        //        FileStream fs = null;
        //        List<Data> dataList = new List<Data>();
        //        GridBuilderProjectData importData = new GridBuilderProjectData();
        //        List<ICommand> commandList = new List<ICommand>();

        //        try
        //        {
        //            fs = new FileStream(url, FileMode.Open);

        //            importData = (GridBuilderProjectData)BFormatter.Deserialize(fs);
        //            dataList = importData.data;

        //            foreach (Data data in dataList)
        //            {
        //                ICommand command = null;

        //                //Select Command Type
        //                if (data.isErase)
        //                {
        //                    command = CreateCommand(typeof(EraseObjectCommand), data);
        //                }
        //                else
        //                {
        //                    command = CreateCommand(typeof(PlaceObjectCommand), data);
        //                }

        //                commandList.Add(command);
        //            }
        //        }
        //        catch (System.Exception e)
        //        {
        //            Debug.Log("Serialization Error: " + e.Message + " + " + e.StackTrace);
        //        }

        //        Execute(commandList);
        //    }

        //    private static ICommand CreateCommand(Type _classType, Data _data)
        //    {
        //        //Get Prefab
        //        string assetPath = AssetDatabase.GUIDToAssetPath(_data.PrefabName);
        //        GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
        //        Debug.Log(assetPath);
        //        if (prefab == null) { Debug.Log("Error on importing: " + assetPath); return null; }

        //        //Create Command
        //        ICommand command = (ICommand)Activator.CreateInstance(
        //            _classType,
        //            prefab,
        //            new Vector3(
        //                _data.positionX,
        //                _data.positionY,
        //                _data.positionZ),
        //            new Quaternion(
        //                _data.rotationX,
        //                _data.rotationY,
        //                _data.rotationZ,
        //                _data.rotationW)
        //        );

        //        return command;
        //    }

        //    public void Execute(List<ICommand> _commands)
        //    {
        //        Dictionary<Vector3, GameObject> _gridDictionary = new Dictionary<Vector3, GameObject>();

        //        foreach (ICommand command in _commands)
        //        {
        //            command.Execute(_gridDictionary);
        //        }
        //    }
    }
}