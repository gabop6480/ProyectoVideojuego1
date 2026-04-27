using UnityEngine;
using Unity.Cinemachine;

public class ZonaTransicion : MonoBehaviour
{
    public CinemachineCamera camaraNivel1;
    public CinemachineCamera camaraNivel2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Si la posición X de Umbra es menor a la del sensor, viene del Nivel 1 hacia el 2
            if (other.transform.position.x < transform.position.x)
            {
                ActivarCamara2();
            }
            // Si la posición X de Umbra es mayor, viene regresando del Nivel 2 al 1
            else
            {
                ActivarCamara1();
            }
        }
    }

    public void ActivarCamara1()
    {
        camaraNivel1.Priority = 100;
        camaraNivel2.Priority = 0;
        Debug.Log("Regresando al Nivel 1");
    }

    public void ActivarCamara2()
    {
        camaraNivel1.Priority = 0;
        camaraNivel2.Priority = 100;
        Debug.Log("Entrando al Nivel 2");
    }
}