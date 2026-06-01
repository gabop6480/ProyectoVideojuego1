using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class DeteccionLuz : MonoBehaviour
{
    [Header("Configuración de Daño")]
    [Tooltip("Tiempo en segundos que Umbra puede estar bajo la luz antes de morir")]
    public float tiempoMaximoBajoLuz = 2.0f;

    [Header("Capas de Colisión")]
    [Tooltip("Selecciona las capas que representan muros o suelos que bloquean la luz")]
    public LayerMask capasQueBloqueanLuz;

    [Header("Juiciness & Feedback (Efectos)")]
    [Tooltip("Arrastra aquí el Particle System de humo/ceniza colocado en la escena")]
    public ParticleSystem particulasQuemadura;

    [Tooltip("Arrastra aquí el objeto 'GeneradorDeTemblores' que tiene el Cinemachine Impulse Source")]
    public CinemachineImpulseSource fuenteImpulso;

    private float temporizador = 0f;
    private bool estaDentroDelArea = false;
    private Transform transformJugador;

    void Start()
    {
        // Sistema de desfase aleatorio para las animaciones de giro
        Animator anim = GetComponent<Animator>();
        if (anim != null)
        {
            float desfaseAleatorio = Random.Range(0f, 1f);
            anim.Play("LightMove", 0, desfaseAleatorio);
        }

        if (particulasQuemadura != null)
        {
            particulasQuemadura.Stop();
        }
    }

    void Update()
    {
        if (estaDentroDelArea && transformJugador != null)
        {
            // Verificamos si está REALMENTE expuesto (sin muros de por medio)
            if (EstaRealmenteExpuesto())
            {
                temporizador += Time.deltaTime;
                float porcentajeQuemadura = temporizador / tiempoMaximoBajoLuz;

                // --- EFECTO 1: FEEDBACK VISUAL (Solo color y parpadeo rápido) ---
                SpriteRenderer sr = transformJugador.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    float parpadeo = Mathf.Sin(Time.time * 40f) > 0 ? 1f : 0.3f;
                    sr.color = Color.Lerp(Color.white, Color.red, porcentajeQuemadura) * parpadeo;
                }

                // --- EFECTO 2: SISTEMA DE PARTÍCULAS ---
                if (particulasQuemadura != null)
                {
                    particulasQuemadura.transform.position = transformJugador.position;
                    if (!particulasQuemadura.isPlaying)
                    {
                        particulasQuemadura.Play();
                    }
                }

                // --- EFECTO 3: CINEMACHINE CAMERA SHAKE ---
                if (fuenteImpulso != null)
                {
                    // Error tipográfico corregido de la versión anterior: fuenceImpulso -> fuenteImpulso
                    fuenteImpulso.GenerateImpulseWithVelocity(Random.insideUnitCircle * porcentajeQuemadura * 0.05f);
                }

                if (temporizador >= tiempoMaximoBajoLuz)
                {
                    ReiniciarNivel();
                }
            }
            else
            {
                // Si se esconde detrás de un muro, los efectos se limpian inmediatamente
                ResetearEstado();
            }
        }
    }

    bool EstaRealmenteExpuesto()
    {
        Vector2 origenLuz = transform.position;
        Vector2 posicionJugador = transformJugador.position;
        Vector2 direccion = (posicionJugador - origenLuz).normalized;
        float distancia = Vector2.Distance(origenLuz, posicionJugador);

        RaycastHit2D hit = Physics2D.Raycast(origenLuz, direccion, distancia, capasQueBloqueanLuz);
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
            if (sr != null)
            {
                sr.color = Color.white; // Restaura su color normal
            }
        }

        if (particulasQuemadura != null) particulasQuemadura.Stop();
    }

    void ReiniciarNivel()
    {
        Debug.Log("¡Umbra se ha disuelto en la luz!");
        ResetearEstado();

        /*ControladorUIJuego uiJuego = Object.FindFirstObjectByType<ControladorUIJuego>();
        if (uiJuego != null)
        {
            uiJuego.MostrarDerrota();
        }
        else
        {*/
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //}
    }
}