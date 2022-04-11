using System.Collections;
using UnityEngine;


public class EraseButton : MonoBehaviour
{
    public GridBuilder gridBuilder {get; set; }

    public void ToggleErase()
    {
        gridBuilder.controls.eraseMode = !gridBuilder.controls.eraseMode;
    }
}