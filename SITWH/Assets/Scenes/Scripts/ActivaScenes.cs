using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para SceneManager

public class ActivaScenes : MonoBehaviour
{
    // Método público que se llamará desde el botón
    public void CargarSampleScene()
    {
        // Carga la escena llamada "SampleScene"
        SceneManager.LoadScene("SampleScene");
    }
}