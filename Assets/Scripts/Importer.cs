using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEditor;
using System;

public class Importer :MonoBehaviour
{
    public static List<ICommand> Import(string fileName)
    {
        Debug.Log("Importing");

        string url = Path.Combine(Application.persistentDataPath, fileName);

        BinaryFormatter BFormatter = new BinaryFormatter();
        FileStream fs = null;
        List<Data> dataList = new List<Data>();
        List<ICommand> commandList = new List<ICommand>();

        try
        {
            fs = new FileStream(url, FileMode.Open);

            dataList = (List<Data>)BFormatter.Deserialize(fs);

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
            Debug.LogError("Serialization Error: " + e.Message);
        }

        return commandList;
    }

    private static ICommand CreateCommand(Type _classType, Data _data)
    {
        //Get Prefab
        string assetPath = AssetDatabase.GUIDToAssetPath(_data.GUID);
        GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
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