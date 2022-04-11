using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIButtons : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldExport;
    [SerializeField] private TMP_InputField inputFieldPrefabs;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject gridBuilder;

    public void ToggleSettingsMenu()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

    public void BrowsePrefabs()
    {
        string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        inputFieldPrefabs.text = directory;
        gridBuilder.GetComponent<GridBuilder>().prefabsPath = directory;
    }

    public void BrowseExport()
    {
        string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        inputFieldExport.text = directory;
        gridBuilder.GetComponent<GridBuilder>().exportPath = directory;
    }

    public void ImportButton()
    {
        string directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        gridBuilder.GetComponent<GridBuilder>().ImportGrid(directory);
    }

    public void ExportButton()
    {
        gridBuilder.GetComponent<GridBuilder>().ExportGrid();
    }
}