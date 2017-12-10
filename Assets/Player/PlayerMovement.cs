using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkMoveStopRadius = 0.2f;
    [SerializeField] private float attackMoveStopRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object. ����� �տ� m_�� ���ΰŴ�. 
    CameraRaycaster cameraRaycaster;
    private Vector3 currentDestination, clickPoint;

    private bool isInDirectMode = false; // TODO consider making static later (�̷��� �ϴ� ������ ����� static�� ������ ����μ��� private �����ε� ����ϱ� ����)

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)) //G for gamepad. TODO add to menu. 
        {
            isInDirectMode = !isInDirectMode; //toggle mode
            currentDestination = transform.position;    //clear the clickTarget. �� ���� �߰��ؾ� ���콺 -> Ű���� -> ���콺�� �ٲ��� �� ���� ���콺 ��ҷ� ĳ���Ͱ� �̵����� �ʴ´�. 
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
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    break;
                default:
                    print("Unexpected layer found");
                    return;
            }
        }

        WalkToDestination();
    }

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharacter.Move(currentDestination - transform.position, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    void OnDrawGizmos()
    {
        //Draw movement Gizmos
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, clickPoint);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.2f);

        //Draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}

