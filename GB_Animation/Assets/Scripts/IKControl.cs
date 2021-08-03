using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    [SerializeField]
    private string _nameLegLeft;

    [SerializeField]
    private string _nameLegRight;

    [SerializeField]
    private bool _isActiveIK;

    [SerializeField]
    private Transform _lookObject;

    [SerializeField]
    private Transform _pointLeftHandObject;

    [SerializeField]
    private Transform _pointRightHandObject;

    [SerializeField]
    private float _valueWeight;

    [SerializeField]
    private LayerMask _rayLayer;

    [SerializeField]
    private Transform _footTransform;

    [SerializeField]
    private Vector3 _offsetFootL;

    [SerializeField]
    private Vector3 _offsetFootR;

    [SerializeField]
    private int _trackingDistance;

    [SerializeField]
    private Transform _headPosition;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {

        if (_isActiveIK)
        {
            if(canTracking())
            {
                _animator.SetLookAtWeight(_valueWeight);
                _animator.SetLookAtPosition(_lookObject.position);
            }
            else
            {
                _animator.SetLookAtWeight(0);
            }

            ChangeWeightRightHand(_valueWeight);
            _animator.SetIKPosition(AvatarIKGoal.RightHand, _pointRightHandObject.position);
            _animator.SetIKRotation(AvatarIKGoal.RightHand, _pointRightHandObject.rotation);

            ChangeWeightLeftHand(_valueWeight);
            _animator.SetIKPosition(AvatarIKGoal.LeftHand, _pointLeftHandObject.position);
            _animator.SetIKRotation(AvatarIKGoal.LeftHand, _pointLeftHandObject.rotation);

            IKLeg();
        }
        else
        {
            ChangeWeightRightHand(0);
            ChangeWeightLeftHand(0);
        }
    }

    private bool canTracking()
    {
        float dist = Vector3.Distance(_lookObject.position, _headPosition.position);
        return dist <= _trackingDistance;
    }

    private void IKLeg()
    {
        var weightFootL = _animator.GetFloat(_nameLegLeft);
        var weightFootR = _animator.GetFloat(_nameLegRight);

        _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weightFootL);
        _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weightFootR);

        ChangePositionLegs(AvatarIKGoal.LeftFoot, _offsetFootL);
        ChangePositionLegs(AvatarIKGoal.RightFoot, _offsetFootR);

    }

    private void ChangePositionLegs(AvatarIKGoal avatarIKGoal,Vector3 offset)
    {
        var legPosition = _animator.GetIKPosition(avatarIKGoal);

        if(Physics.Raycast(legPosition + Vector3.up,Vector3.down,out var hit, 3, _rayLayer))
        {
            _animator.SetIKPosition(avatarIKGoal, hit.point + offset);
            _animator.SetIKRotation(avatarIKGoal, _footTransform.rotation);
            _animator.SetIKRotationWeight(avatarIKGoal, 1);
        }
    }

    private void ChangeWeightRightHand(float weight)
    {
        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
    }

    private void ChangeWeightLeftHand(float weight)
    {
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
    }
}
