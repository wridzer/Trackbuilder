using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public abstract class Exporter
{
        //Export scene
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    //Scene newScene = SceneManager.GetActiveScene();
        //    //string createdScene = SceneSerialization.SerializeScene(newScene);
        //    //System.IO.File.WriteAllText(Application.dataPath + "\\newScene.json", createdScene);
        //    //SceneManager.UnloadSceneAsync(newScene);
        //    //AssetDatabase.Refresh();
        //}
    public static void Export(string fileName, List<ICommand> commandList)
    {
        Debug.Log("Exporting");

        List<Data> dataList = new List<Data>();

        foreach(ICommand command in commandList)
        {
            Data data = new Data();
            data.positionX = command.Position.x;
            data.positionY = command.Position.y;
            data.positionZ = command.Position.z;
            data.rotationX = command.Rotation.x;
            data.rotationY = command.Rotation.y;
            data.rotationZ = command.Rotation.z;
            data.rotationW = command.Rotation.w;
            data.GUID = command.Prefab.GetInstanceID();
            dataList.Add(data);
        }

        BinaryFormatter BFormatter = new BinaryFormatter();
        //Assert.IsFalse(string.IsNullOrEmpty(fileName));

        string url = Path.Combine(Application.persistentDataPath, fileName);

        FileStream fs = null;

        try
        {
            fs = new FileStream(url, FileMode.OpenOrCreate);

            BFormatter.Serialize(fs, dataList);

            fs.Flush();
            fs.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Serialization Error: " + e.Message);
        }

        Debug.Log("Done Exporting: " + url);
    }
}