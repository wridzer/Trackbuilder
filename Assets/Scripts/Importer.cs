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
                if(data.GUID == null)
                {
                    //Eraese command here!!
                    continue;
                }
                else
                {
                    //Get Prefab
                    string assetPath = AssetDatabase.GUIDToAssetPath(data.GUID);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                    if (prefab == null) { Debug.Log(assetPath); continue; }
                    //Create Command
                    command = new PlaceObjectCommand(
                        prefab,
                        new Vector3(
                            data.positionX,
                            data.positionY,
                            data.positionZ),
                        new Quaternion(
                            data.rotationX,
                            data.rotationY,
                            data.rotationZ,
                            data.rotationW
                            )); 
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
}