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

    Camera cam;
    float MouseX
    {
        get
        {
            return Input.GetAxisRaw("MouseX");
        }
    }
    float MouseY
    {
        get
        {
            return Input.GetAxisRaw("MouseY");
        }
    }

    [SerializeField] float _sensibilidadX;
    [SerializeField] float _sensibilidadY;

    #region Métodos privados

    void InputDetection()
    {
        //movimiento
        _direccionMovimiento = transform.forward * Vertical + transform.right * Horizontal;

        //camara

    }

    void MovePlayer()
    {
        _rigidbody.AddForce(_direccionMovimiento.normalized * _velocidad * _multiplicadorVelocidad, ForceMode.Acceleration);
    }

    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        InputDetection();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
}
