using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object. ����� �տ� m_�� ���ΰŴ�. 
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    private bool isInDirectMode = false; // TODO consider making static later (�̷��� �ϴ� ������ ����� static�� ������ ����μ��� private �����ε� ����ϱ� ����)

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)) //G for gamepad. TODO add to menu. 
        {
            isInDirectMode = !isInDirectMode; //toggle mode
            currentClickTarget = transform.position;    //clear the clickTarget. �� ���� �߰��ؾ� ���콺 -> Ű���� -> ���콺�� �ٲ��� �� ���� ���콺 ��ҷ� ĳ���Ͱ� �̵����� �ʴ´�. 
        }

        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
            ProcessMouseMovement();
        }        
    }

    private void ProcessDirectMovement() //ThirdPersonUserControl.cs���� ���� �� �������� ���ݾ� �����ߴ�. 
    {
        float h = Input.GetAxis("Horizontal");  //ThirdPersonUserControl.cs���� �����ؿ����� ����Ϸ� ������� �ƴϴϱ� CrossPlatform ��¼���� �ʿ���� �׳� Input���� ��ü�Ѵ�. ����� GetAxis ��ü�� �⺻������ Ű����� �����е� �� �� ���ִ�. 
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * camForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(movement, false, false);
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    break;
                case Layer.Enemy:
                    print("Not moving to enemy");
                    break;
                default:
                    print("Unexpected layer found");
                    return;
            }
        }
        var playerToClickPoint = currentClickTarget - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharacter.Move(currentClickTarget - transform.position, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }
}

