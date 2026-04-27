using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaJuego : MonoBehaviour
{
    [Header("Interfaz de Usuario")]
    [Tooltip("Arrastra aquí el Panel de Victoria que está dentro del Canvas")]
    public GameObject panelVictoriaUI;

    [Header("Configuración de Escenas")]
    [Tooltip("Nombre de la siguiente escena (opcional)")]
    public string nombreSiguienteNivel;

    private void Start()
    {
        // Nos aseguramos de que el panel esté oculto al empezar el nivel
        if (panelVictoriaUI != null)
        {
            panelVictoriaUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si lo que tocó la meta es el jugador
        if (other.CompareTag("Player"))
        {
            Ganar();
        }
    }

    public void Ganar()
    {
        Debug.Log("¡Victoria! Umbra ha llegado a la meta.");

        if (panelVictoriaUI != null)
        {
            panelVictoriaUI.SetActive(true); // Muestra la pantalla de ganar
            Time.timeScale = 0f; // Pausa el tiempo del juego (las luces dejan de rotar)
        }
        else
        {
            Debug.LogWarning("No has asignado el Panel de Victoria en el Inspector.");
        }
    }

    // Función para el botón 'Jugar de nuevo'
    public void ReiniciarNivel()
    {
        Time.timeScale = 1f; // ¡MUY IMPORTANTE! Despausa el juego antes de cargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Función para el botón 'Siguiente Nivel'
    public void SiguienteNivel()
    {
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(nombreSiguienteNivel))
        {
            SceneManager.LoadScene(nombreSiguienteNivel);
        }
    }
}