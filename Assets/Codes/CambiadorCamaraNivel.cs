using UnityEngine;
using Unity.Cinemachine; // Si usas la versión más reciente de Cinemachine en Unity 2026
// Nota: Si usas una versión anterior, el namespace podría ser: using Cinemachine;

public class CambiadorCamaraNivel : MonoBehaviour
{
    [Header("Configuración de la Cámara")]
    [Tooltip("Arrastra aquí tu Cinemachine Virtual Camera")]
    public CinemachineCamera camaraVirtual;

    [Tooltip("El Collider que define los límites de ESTE nivel específico")]
    public Collider2D limitesDeEsteNivel;

    private CinemachineConfiner2D confiner;

    void Start()
    {
        if (camaraVirtual != null)
        {
            // Obtenemos el componente Confiner de la cámara
            confiner = camaraVirtual.GetComponent<CinemachineConfiner2D>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && confiner != null && limitesDeEsteNivel != null)
        {
            // Cambiamos los límites al nuevo nivel
            confiner.BoundingShape2D = limitesDeEsteNivel;

            // TRUCO: Forzamos a Cinemachine a limpiar el caché del colisionador anterior
            // Esto evita que intente calcular la posición usando los dos límites a la vez
            confiner.InvalidateBoundingShapeCache();

            Debug.Log("Límites actualizados limpiamente para: " + gameObject.name);
        }
    }
}