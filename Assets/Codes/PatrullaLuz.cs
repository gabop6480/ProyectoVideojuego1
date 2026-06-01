using UnityEngine;

public class PatrullaLuz : MonoBehaviour
{
    [Header("Puntos de Movimiento")]
    [Tooltip("Coloca aquí los objetos vacíos que definen la ruta de la luz")]
    public Transform[] puntosDeRuta;

    [Header("Configuración de Velocidad")]
    public float velocidad = 3.0f;
    [Tooltip("Tiempo que espera la luz al llegar a un extremo antes de regresar")]
    public float tiempoEspera = 0.5f;

    private int indiceSiguientePunto = 0;
    private float temporizadorEspera = 0f;
    private bool estaEsperando = false;

    void Update()
    {
        if (puntosDeRuta.Length == 0) return;

        // Si está esperando en un punto, cuenta el tiempo
        if (estaEsperando)
        {
            temporizadorEspera += Time.deltaTime;
            if (temporizadorEspera >= tiempoEspera)
            {
                estaEsperando = false;
                temporizadorEspera = 0f;
                // Avanza al siguiente punto de la lista
                indiceSiguientePunto = (indiceSiguientePunto + 1) % puntosDeRuta.Length;
            }
            return;
        }

        // Mueve la luz hacia el punto objetivo actual
        Vector3 posicionObjetivo = puntosDeRuta[indiceSiguientePunto].position;
        transform.position = Vector3.MoveTowards(transform.position, posicionObjetivo, velocidad * Time.deltaTime);

        // Si llega al punto objetivo, activa la espera
        if (Vector3.Distance(transform.position, posicionObjetivo) < 0.1f)
        {
            estaEsperando = true;
        }
    }
}