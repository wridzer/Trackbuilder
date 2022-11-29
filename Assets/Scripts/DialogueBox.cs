using System.Collections;
using TMPro;
using UnityEngine;


public class DialogueBox : MonoBehaviour
{
    public TMP_Text MessageText;

    public void OnOkClick()
    {
        this.gameObject.SetActive(false);
    }
}