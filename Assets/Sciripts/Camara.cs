using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Camara : MonoBehaviour
{
    public enum SecurityCameraState { IDLE, MOVING, TARGETTING };
    [Header("FSM")] public SecurityCameraState cameraState;
    public float currentStateTime;
    public Transform[] posicionesCamara;
    public Transform playerTransform;
    public int puntoCamara;
    public float velocidadCamara = 45;
    public float distanciaVision = 40;
    public float anguloVision = 30;
    public bool veoJugador;
    public Vector3 lastPlayerPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }
    void ChangeState(SecurityCameraState newState)
    {
        ExitState();
        cameraState = newState;
        EnterState(newState);
        Debug.Log("Cámara -> " + cameraState);
    }

    void UpdateState()
    {
        currentStateTime += Time.deltaTime;
        Vector3 detectatPlayer = playerTransform.position - transform.position;
        veoJugador = true;
        if (detectatPlayer.magnitude > distanciaVision)
        {
            veoJugador = false;
        }
        if (Vector3.Angle(transform.forward, detectatPlayer) > anguloVision)
        {
            veoJugador = false;
        }
        switch (cameraState)
        {

            case SecurityCameraState.IDLE:
                if (veoJugador)
                {
                    ChangeState(SecurityCameraState.TARGETTING);
                    break;
                }
                if (currentStateTime >= 2)
                {
                    ChangeState(SecurityCameraState.MOVING);
                }
                break;


            case SecurityCameraState.MOVING:
                if (veoJugador)
                {
                    ChangeState(SecurityCameraState.TARGETTING);
                    break;
                }
                Vector3 direccion = posicionesCamara[puntoCamara].position - transform.position;
                Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, velocidadCamara * Time.deltaTime);
                if (Quaternion.Angle(transform.rotation, rotacionObjetivo) < 0.5f)
                {
                    puntoCamara = (puntoCamara + 1) % posicionesCamara.Length;
                    ChangeState(SecurityCameraState.IDLE);
                }
                break;


            case SecurityCameraState.TARGETTING:
                if (veoJugador) {lastPlayerPosition = playerTransform.position;}              
                Vector3 direccionPlayer = lastPlayerPosition - transform.position;
                Quaternion rotacionPlayer = Quaternion.LookRotation(direccionPlayer);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacionPlayer, velocidadCamara * Time.deltaTime);
                if (!veoJugador && Quaternion.Angle(transform.rotation, rotacionPlayer) < 0.5f)
                {
                    ChangeState(SecurityCameraState.IDLE);
                }

                break;
        }
    }

    void EnterState(SecurityCameraState newState)
    {

        currentStateTime = 0;
        switch (newState)
        {
            case SecurityCameraState.IDLE:
                break;
            case SecurityCameraState.MOVING:
                break;
            case SecurityCameraState.TARGETTING:
                lastPlayerPosition = playerTransform.position;
                break;

        }
    }

    void ExitState()
    {
        switch (cameraState)
        {
            case SecurityCameraState.IDLE:
                break;
            case SecurityCameraState.MOVING:
                break;
            case SecurityCameraState.TARGETTING:
                break;
        }
    }
}
