using UnityEngine;
using System.Collections;

public class Win : MonoBehaviour {

    public Texture winTexture;

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), winTexture);
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 150, 25), "Don't try again, you are going to lose it anyway, because you are a LOOSER!"))
        {
            Application.LoadLevel("WinScreen");
        }
        if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 25, 150, 25), "Run away, you coward! That's the only thing that you can do here."))
        {
            Application.Quit();
        }
    }
}
