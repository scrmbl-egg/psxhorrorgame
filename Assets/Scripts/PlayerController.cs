using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Propiedades")]

    [SerializeField] int _vidaMax = 100;
    float _vida;
    public float Vida
    {
        get
        {
            return Mathf.Clamp(_vida, 0, _vidaMax);
        }
        set
        {
            var vida = _vida;

            _vida = Mathf.Clamp(value, 0, _vidaMax);

            if (vida < _vida) Curar();
            else if (vida > _vida) Dmg();
        }
    }

    void Curar()
    {

    }

    void Dmg()
    {

    }

    [Header("Componentes")]

    Rigidbody _rigidbody;

    [Header("Movimiento")]

    [SerializeField] float _velocidad;
    int _multiplicadorVelocidad = 10;
    Vector3 _direccionMovimiento;
    float Vertical
    {
        get
        {
            return Input.GetAxisRaw("Vertical");
        }
    }
    float Horizontal
    {
        get
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }

    [Header("Control de cámara")]

    public Camera cam;

    [SerializeField] float _sensibilidad = 10;

    [SerializeField] float _multiplicadorSensibilidad = 10;

    float _rotacionX;
    float _rotacionY;

    float MouseX
    {
        get
        {
            return Input.GetAxisRaw("Mouse X");
        }
    }
    float MouseY
    {
        get
        {
            return Input.GetAxisRaw("Mouse Y");
        }
    }

    #region Métodos privados

    void InputDetection()
    {
        //movimiento
        _direccionMovimiento = transform.forward * Vertical + transform.right * Horizontal;

        //camara
        _rotacionY += MouseX * _sensibilidad * _multiplicadorSensibilidad;
        _rotacionX -= MouseY * _sensibilidad * _multiplicadorSensibilidad;

        _rotacionX = Mathf.Clamp(_rotacionX, -90f, 90f);
    }

    void MovePlayer()
    {
        _rigidbody.AddForce(_direccionMovimiento.normalized * _velocidad * _multiplicadorVelocidad, ForceMode.Acceleration);
    }

    void RotacionCamara()
    {
        cam.transform.localRotation = Quaternion.Euler(_rotacionX, 0, 0);
        transform.rotation = Quaternion.Euler(0, _rotacionY, 0);
    }

    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        InputDetection();
        RotacionCamara();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
}
