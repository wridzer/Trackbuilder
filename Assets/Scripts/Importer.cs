using System.Collections;
using Unity.RuntimeSceneSerialization;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Importer : MonoBehaviour
{
    public string datapath;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Import(datapath);
        }
    }

    public void Import(string _datapath)
    {
        //string json = System.IO.File.ReadAllText(_datapath);
        //Scene newScene = SceneSerialization.FromJson<Scene>(json);
        //Debug.Log(newScene.GetRootGameObjects());
        ////SceneManager.SetActiveScene(newScene);
        ////SceneManager.LoadScene(newScene);
    }
}