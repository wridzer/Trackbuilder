using System.Collections;
using System;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Diagnostics;
using UnityEditor;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldExport, inputFieldPrefabs, dimensionW, dimensionH, dimensionL;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject gridBuilder;
    [SerializeField] private GameObject buildButton;

    public void ToggleSettingsMenu()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    //public async void BrowsePrefabs()
    //{
    //    Process process =  Process.Start("explorer.exe", "/select,");
    //    process.WaitForInputIdle();
    //    string directory = process.StandardOutput.ToString();
    //    //string directory = EditorUtility.OpenFilePanel("Select Directory", "", "");
    //    inputFieldPrefabs.text = directory;
    //    string fileName = Path.GetFileName(directory);
    //    gridBuilder.GetComponent<GridBuilder>().assetBundleName = fileName;
    //    OnValueChanged();
    //}

    //public void BrowseExport()
    //{
    //    string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
    //    inputFieldExport.text = directory;
    //    gridBuilder.GetComponent<GridBuilder>().exportPath = directory;
    //}

    //public void BrowseImport()
    //{
    //    string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
    //    inputFieldExport.text = directory;
    //    //clear existing grid
    //    gridBuilder.GetComponent<GridBuilder>().ImportGrid(directory);
    //    settingsMenu.SetActive(false);
    //}

    public void OnValueChanged()
    {
        var builderScript = gridBuilder.GetComponent<GridBuilder>();
        if (builderScript.gridWidth != 0 && builderScript.gridHeight != 0 && builderScript.gridLength != 0 && inputFieldPrefabs != null)
        {
            buildButton.GetComponent<Button>().interactable = true;
        }
        builderScript.gridWidth = int.Parse(dimensionW.text);
        builderScript.gridHeight = int.Parse(dimensionH.text);
        builderScript.gridLength = int.Parse(dimensionL.text);
    }

    public void GenerateGrid()
    {
        //clear existing grid
        gridBuilder.GetComponent<GridBuilder>().BuildGrid();
        settingsMenu.SetActive(false);
    }

    public void ImportButton(TMP_InputField _inputfield)
    {
        string directory = _inputfield.text;
        gridBuilder.GetComponent<GridBuilder>().ImportGrid(directory);
        settingsMenu.SetActive(false);
        if (Directory.Exists(directory))
        {
        } else
        {
            EditorUtility.DisplayDialog("Error", "Please enter a valid import path", "Ok");
        }
    }

    public void ExportButton(TMP_InputField _inputfield)
    {
        string directory = _inputfield.text;
        if (Directory.Exists(directory))
        {
            gridBuilder.GetComponent<GridBuilder>().exportPath = directory;
            gridBuilder.GetComponent<GridBuilder>().ExportGrid();
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Please enter a valid export path", "Ok");
        }
    }

    public void PrefabButton(TMP_InputField _inputfield)
    {
        string directory = _inputfield.text;
        try
        {
            string fileName = Path.GetFileName(directory);
            gridBuilder.GetComponent<GridBuilder>().assetBundleName = fileName;
            OnValueChanged();
        }
        catch (Exception ex)
        {
            EditorUtility.DisplayDialog("Error", "Please enter a valid prefabs path", "Ok");
        }
    }
}