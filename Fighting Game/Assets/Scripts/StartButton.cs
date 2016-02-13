using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {

    public void StartClicked()
    {
            Application.LoadLevel("Main");
    }
}
