using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEditor;
using System;

public class Importer :MonoBehaviour
{
    public static List<ICommand> Import(string fileName, GridBuilder _builderInstance)
    {
        Debug.Log("Importing");

        string url = Path.Combine(Application.persistentDataPath, fileName);

        BinaryFormatter BFormatter = new BinaryFormatter();
        FileStream fs = null;
        List<Data> dataList = new List<Data>();
        Settings settings = new Settings();
        GridBuilderProjectData importData = new GridBuilderProjectData();
        List<ICommand> commandList = new List<ICommand>();

        try
        {
            fs = new FileStream(url, FileMode.Open);

            importData = (GridBuilderProjectData)BFormatter.Deserialize(fs);
            dataList = importData.data;
            settings = importData.settings;

            //Import gridobjects
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

            //Import gridsettings
             _builderInstance.assetBundleName = settings.assetBundleName;
             _builderInstance.exportPath = settings.exportPath;
             _builderInstance.gridHeight = settings.gridHeight;
             _builderInstance.gridWidth = settings.gridWidth;
             _builderInstance.gridLength = settings.gridLength;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Serialization Error: " + e.Message);
        }

        return commandList;
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
}