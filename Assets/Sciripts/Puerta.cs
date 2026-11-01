using UnityEngine;

public class Puerta : MonoBehaviour
{
    public enum EstadoPuerta { CERRADA, ABRIENDO, ABIERTA, CERRANDO }

    [SerializeField] Transform transformVisagra;
    [SerializeField] float anguloAbierto = 90f;
    [SerializeField] float velocidadGiro = 90f;
    [SerializeField] string tagJugador = "player";

    EstadoPuerta estadoActual = EstadoPuerta.CERRADA;
    float anguloActual = 0f;
    bool jugadorDentro = false;

    private void Start()
    {
        EnterState();
    }

    private void Update()
    {
        UpdateCurrentState();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador))
            jugadorDentro = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagJugador))
            jugadorDentro = false;
    }


    void ChangeState(EstadoPuerta nuevoEstado)
    {
        ExitState();
        estadoActual = nuevoEstado;
        EnterState();
        Debug.Log("Puerta -> " + estadoActual);
    }

    void EnterState()
    {
        switch (estadoActual)
        {
            case EstadoPuerta.CERRADA:
                anguloActual = 0f;
                AplicarRotacion();
                break;
            case EstadoPuerta.ABRIENDO:
                break;
            case EstadoPuerta.ABIERTA:
                anguloActual = anguloAbierto;
                AplicarRotacion();
                break;
            case EstadoPuerta.CERRANDO:
                break;
        }
    }

    void UpdateCurrentState()
    {
        switch (estadoActual)
        {
            case EstadoPuerta.CERRADA:
                if (jugadorDentro)
                    ChangeState(EstadoPuerta.ABRIENDO);
                break;

            case EstadoPuerta.ABRIENDO:
                anguloActual = Mathf.MoveTowards(anguloActual, anguloAbierto, velocidadGiro * Time.deltaTime);
                AplicarRotacion();
                if (Mathf.Approximately(anguloActual, anguloAbierto))
                    ChangeState(EstadoPuerta.ABIERTA);
                break;

            case EstadoPuerta.ABIERTA:
                if (!jugadorDentro)
                    ChangeState(EstadoPuerta.CERRANDO);
                break;

            case EstadoPuerta.CERRANDO:
                anguloActual = Mathf.MoveTowards(anguloActual, 0f, velocidadGiro * Time.deltaTime);
                AplicarRotacion();
                if (Mathf.Approximately(anguloActual, 0f))
                    ChangeState(EstadoPuerta.CERRADA);
                break;
        }
    }

    void ExitState()
    {
        switch (estadoActual)
        {
            case EstadoPuerta.CERRADA:
                break;
            case EstadoPuerta.ABRIENDO:
                break;
            case EstadoPuerta.ABIERTA:
                break;
            case EstadoPuerta.CERRANDO:
                break;
        }
    }

    void AplicarRotacion()
    {
        transformVisagra.localRotation = Quaternion.Euler(0f, anguloActual, 0f);
    }
}