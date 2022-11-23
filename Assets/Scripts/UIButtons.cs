using System.Collections;
using System;
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
    [SerializeField] private DialogueBox DialogueBox;

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
        if (File.Exists(directory) && directory != null)
        {
            gridBuilder.GetComponent<GridBuilder>().ImportGrid(directory);
            settingsMenu.SetActive(false);
        } else
        {
            DialogueBox.gameObject.SetActive(true);
            DialogueBox.MessageText.text = "Please enter a valid import path";
        }
    }

    public void ExportButton(TMP_InputField _inputfield)
    {
        string directory = _inputfield.text;
        if (Directory.Exists(directory) && directory != null)
        {
            gridBuilder.GetComponent<GridBuilder>().exportPath = directory;
            gridBuilder.GetComponent<GridBuilder>().ExportGrid();
        }
        else
        {
            DialogueBox.gameObject.SetActive(true);
            DialogueBox.MessageText.text = "Please enter a valid export path";
        }
    }

    public void PrefabButton(TMP_InputField _inputfield)
    {
        string directory = _inputfield.text;
        try
        {
            string fileName = Path.GetFileName(directory);
            gridBuilder.GetComponent<GridBuilder>().assetBundlePath = directory;
            OnValueChanged();
        }
        catch (Exception ex)
        {
            DialogueBox.gameObject.SetActive(true);
            DialogueBox.MessageText.text = "Please enter a valid prefabs path";
        }
    }
}