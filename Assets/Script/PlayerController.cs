using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _controller;
    private Transform _camera;
    private float _horizontal;
    private float _vertical;
    Animator _animator;

    //variable para saltar y gravedad

    [SerializeField] private float playerSpeed = 5; 
    [SerializeField] private float _jumpHeight = 1;

    private float _gravity = -9.81f;
    private Vector3 _playerGravity;

    //Variables para rotacion

     private float turnSmoothVeloity;

    [SerializeField] float turnSmoothTime = 0.1f;

    //variable para sensor

    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;

    private GameManager _gameManager;

    // Start is called before the first frame update
    
    void Awake()
    {
        _controller = GetComponent <CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main.transform;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical =Input.GetAxisRaw("Vertical");

        if(Input.GetButton("Fire2") && _gameManager._death == false)
        {
             AimMovement();
        }
        else if (_gameManager._death == false)
        {
            Movement();    
        }
        

        if(_gameManager._death == false)
        {
            Jump();
        }

       Die();
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelX", 0);    
        _animator.SetFloat("VelZ", direction.magnitude);


      if(direction !=  Vector3.zero)
        {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVeloity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(direction * playerSpeed * Time.deltaTime);
        }
 
    }

    void Jump()
    {

        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
       
        if(_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = 0;
        }

        if(_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        _playerGravity.y += _gravity * Time.deltaTime;

        _controller.Move(_playerGravity * Time.deltaTime);

    }

      void AimMovement()
    {



        Vector3 direction = new Vector3(_horizontal, 0, _vertical);

        _animator.SetFloat("VelX", _horizontal);    
        _animator.SetFloat("VelZ", _vertical);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y,_camera.eulerAngles.y, ref turnSmoothVeloity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);


      if(direction !=  Vector3.zero)
        {

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _controller.Move(moveDirection * playerSpeed * Time.deltaTime);
        }
    }

     void Die()
        {
            if(Input.GetKey(KeyCode.P) &&  _gameManager._death == false)
            {
                _gameManager.PlayerDeath();
                //_animator.SetTrigger ("IsDeath");     
            }
        }
}