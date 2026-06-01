using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BordeMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si lo que cayó al vacío es el jugador
        if (other.CompareTag("Player"))
        {
            MorirPorCaida();
        }
    }

    void MorirPorCaida()
    {
        //Debug.Log("Umbra cayó al vacío.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Intentamos buscar primero el controlador de UI Document por si ya está activo
        //ControladorUIJuego uiJuego = Object.FindFirstObjectByType<ControladorUIJuego>();

        /*if (uiJuego != null)
        {
            uiJuego.MostrarDerrota();
        }
        else
        {
        // Si no encuentra la UI, reinicia el nivel directamente para poder seguir jugando
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }*/
    }
}
