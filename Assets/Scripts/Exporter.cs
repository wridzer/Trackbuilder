using System.Collections;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class Exporter
{
    public static void Export(string fileName, string filePath, List<ICommand> commandList, GridBuilder _builderInstance)
    {
        Debug.Log("Exporting");

        //Export Grid items
        List<Data> dataList = new List<Data>();
        foreach(ICommand command in commandList)
        {
            Data data = new Data();

            if(command.isErase == true) { data.isErase = true; }
            else { data.isErase = false; }

            data.positionX = command.Position.x;
            data.positionY = command.Position.y;
            data.positionZ = command.Position.z;
            data.rotationX = command.Rotation.x;
            data.rotationY = command.Rotation.y;
            data.rotationZ = command.Rotation.z;
            data.rotationW = command.Rotation.w;
            //data.PrefabName = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(command.Prefab));
            data.PrefabName = command.Prefab.name;
            dataList.Add(data);
        }

        //Export gridsettings
        Settings settings = new Settings();
        settings.assetBundlePath = _builderInstance.assetBundlePath;
        settings.exportPath = _builderInstance.exportPath;
        settings.gridHeight = _builderInstance.gridHeight;
        settings.gridWidth = _builderInstance.gridWidth;
        settings.gridLength = _builderInstance.gridLength;

        //Create exportable file
        GridBuilderProjectData exportData = new GridBuilderProjectData();
        exportData.data = dataList;
        exportData.settings = settings;

        BinaryFormatter BFormatter = new BinaryFormatter();

        string url = Path.Combine(filePath, fileName);

        FileStream fs = null;

        try
        {
            fs = new FileStream(url, FileMode.OpenOrCreate);

            BFormatter.Serialize(fs, exportData);

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