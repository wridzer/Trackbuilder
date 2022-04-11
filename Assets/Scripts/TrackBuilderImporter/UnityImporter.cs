using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace TrackBuilder
{
    public class UnityImporter : MonoBehaviour
    {
        [SerializeField] private TextAsset importFile;

        [ContextMenu("Import Track")]
        public void Import()
        {
            Debug.Log("Importing");

            string url = Path.Combine(AssetDatabase.GetAssetPath(importFile));

            BinaryFormatter BFormatter = new BinaryFormatter();
            FileStream fs = null;
            List<Data> dataList = new List<Data>();
            GridBuilderProjectData importData = new GridBuilderProjectData();
            List<ICommand> commandList = new List<ICommand>();

            try
            {
                fs = new FileStream(url, FileMode.Open);

                importData = (GridBuilderProjectData)BFormatter.Deserialize(fs);
                dataList = importData.data;

                foreach (Data data in dataList)
                {
                    ICommand command = null;

                    //Select Command Type
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
                Debug.Log("Serialization Error: " + e.Message + " + " + e.StackTrace);
            }

            Execute(commandList);
        }

        private static ICommand CreateCommand(Type _classType, Data _data)
        {
            //Get Prefab
            string assetPath = AssetDatabase.GUIDToAssetPath(_data.GUID);
            GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            Debug.Log(assetPath);
            if (prefab == null) { Debug.Log("Error on importing: " + assetPath); return null; }

            //Create Command
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

        public void Execute(List<ICommand> _commands)
        {
            Dictionary<Vector3, GameObject> _gridDictionary = new Dictionary<Vector3, GameObject>();

            foreach (ICommand command in _commands)
            {
                command.Execute(_gridDictionary);
            }
        }
    }
}