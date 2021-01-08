using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotSaver : MonoBehaviour
{
    // file path where images are saved
    public string PATH = "Assets/Resources/Screenshots/";


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            StartCoroutine(RenderScreen());
    }

    // Renders and saves the current view
    private IEnumerator RenderScreen()
    {
        yield return new WaitForEndOfFrame();

        // create texture of screenshot
        Texture2D image = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        image.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        image.Apply();

        // save Texture2D as PNG
        byte[] bytes = ImageConversion.EncodeToPNG(image);
        string name = System.DateTime.Now.ToString().Replace(' ', '_').Replace(".", "").Replace(":", "");
        System.IO.File.WriteAllBytes(PATH + name + ".png", bytes);
        Debug.Log("[Screenshot] Saved image: " + name);
    }
}
