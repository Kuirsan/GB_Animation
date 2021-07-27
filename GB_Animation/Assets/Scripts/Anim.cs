using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Anim : MonoBehaviour
{
    [SerializeField]
    private string _speedAnimation;

    [SerializeField]
    private string _walkSpeedAnimation;

    [SerializeField]
    private string _turnSpeedAnimation;

    [SerializeField]
    private string _isTurnAnimation;

    [SerializeField]
    private float _speed;

    private Animator _animator;

    private int _speedAnimationHash;

    private int _walkSpeedAnimationHash;

    private int _turnSpeedAnimationHash;

    private int _isTurnAnimationHash;

    private float _walkSpeed;

    private float _turnSpeed;

    private float _maxWalkSpeed = 2f;

    private float _maxTurnSpeed = 2f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _speedAnimationHash = Animator.StringToHash(_speedAnimation);
        _walkSpeedAnimationHash = Animator.StringToHash(_walkSpeedAnimation);
        _turnSpeedAnimationHash = Animator.StringToHash(_turnSpeedAnimation);
        _isTurnAnimationHash = Animator.StringToHash(_isTurnAnimation);
    }

    private void Start()
    {
        _animator.SetFloat(_speedAnimationHash, 0f);
    }

    private void Move(float moveSpeed)
    {
        _animator.SetFloat(_speedAnimationHash, moveSpeed);
        if (moveSpeed > -_walkSpeed)
        {
            _animator.SetInteger(_walkSpeedAnimationHash, 1);
        }
        else
        {
            Stop();
        }
    }

    private void Rotate(float turnSpeed)
    {

        if (turnSpeed == 0)
        {
            _animator.SetBool(_isTurnAnimationHash, false);
            _animator.SetFloat(_turnSpeedAnimationHash, turnSpeed);
        }
        else
        {
            _animator.SetFloat(_turnSpeedAnimationHash, turnSpeed);
            _animator.SetBool(_isTurnAnimationHash, true);
        }
    }

    private void Stop()
    {
        _animator.SetInteger(_walkSpeedAnimationHash, 0);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _walkSpeed += _speed * Time.deltaTime;
            _walkSpeed = _walkSpeed >= _maxWalkSpeed ? _maxWalkSpeed : _walkSpeed;
            Move(_walkSpeed);
        }
        else
        {
            _walkSpeed -= _speed * Time.deltaTime;
            _walkSpeed = _walkSpeed <=-_walkSpeed ? 0 : _walkSpeed;
            Move(_walkSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _turnSpeed -= _speed * Time.deltaTime;
            _turnSpeed = _turnSpeed <= -_maxTurnSpeed ? -_maxTurnSpeed : _turnSpeed;
            Rotate(_turnSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _turnSpeed += _speed * Time.deltaTime;
            _turnSpeed = _turnSpeed >= _maxTurnSpeed ? _maxTurnSpeed : _turnSpeed;
            Rotate(_turnSpeed);
        }
        else
        {
            if(_turnSpeed>0)
            {
                if(_turnSpeed> _speed * Time.deltaTime)
                {
                    _turnSpeed -= _speed * Time.deltaTime;
                }
                else
                {
                    _turnSpeed = 0;
                }
            }
            else if(_turnSpeed<0)
            {
                if (Mathf.Abs(_turnSpeed) > _speed * Time.deltaTime)
                {
                    _turnSpeed += _speed * Time.deltaTime;
                }
                else
                {
                    _turnSpeed = 0;
                }
            }

            Rotate(_turnSpeed);
        }
    }
}
