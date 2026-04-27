using UnityEngine;
using UnityEngine.SceneManagement;

public class DeteccionLuz : MonoBehaviour
{
    [Header("Configuración de Daño")]
    [Tooltip("Tiempo en segundos que Umbra puede estar bajo la luz antes de morir")]
    public float tiempoMaximoBajoLuz = 2.0f;

    [Header("Capas de Colisión")]
    [Tooltip("Selecciona las capas que representan muros o suelos que bloquean la luz")]
    public LayerMask capasQueBloqueanLuz;

    private float temporizador = 0f;
    private bool estaDentroDelArea = false;
    private Transform transformJugador;

    void Update()
    {
        if (estaDentroDelArea && transformJugador != null)
        {
            if (EstaRealmenteExpuesto())
            {
                temporizador += Time.deltaTime;

                // Efecto visual opcional: Umbra se vuelve más rojo mientras se quema
                SpriteRenderer sr = transformJugador.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = Color.Lerp(Color.white, Color.red, temporizador / tiempoMaximoBajoLuz);
                }

                if (temporizador >= tiempoMaximoBajoLuz)
                {
                    ReiniciarNivel();
                }
            }
            else
            {
                ResetearEstado();
            }
        }
        else
        {
            ResetearEstado();
        }
    }

    // Verifica si hay un muro entre la lámpara y el jugador
    bool EstaRealmenteExpuesto()
    {
        Vector2 origenLuz = transform.position;
        Vector2 posicionJugador = transformJugador.position;
        Vector2 direccion = (posicionJugador - origenLuz).normalized;
        float distancia = Vector2.Distance(origenLuz, posicionJugador);

        // Lanzamos un rayo invisible (Raycast)
        RaycastHit2D hit = Physics2D.Raycast(origenLuz, direccion, distancia, capasQueBloqueanLuz);

        // Si el rayo NO golpea nada, el camino está despejado -> Umbra está expuesto
        return hit.collider == null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            estaDentroDelArea = true;
            transformJugador = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            estaDentroDelArea = false;
            ResetearEstado();
        }
    }

    void ResetearEstado()
    {
        temporizador = 0f;
        if (transformJugador != null)
        {
            SpriteRenderer sr = transformJugador.GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = Color.white; // Vuelve a su color normal
        }
    }

    void ReiniciarNivel()
    {
        Debug.Log("¡Umbra se ha disuelto en la luz!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}