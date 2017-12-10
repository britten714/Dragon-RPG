using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    

    [SerializeField] private float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object. 멤버라서 앞에 m_를 붙인거다. 
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;

    private bool isInDirectMode = false; // TODO consider making static later (이렇게 하는 이유는 개념상 static이 맞지만 현재로서는 private 만으로도 충분하기 때문)

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    //TODO fix issue with click to move and WSAD conflicting and increasing speed.

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)) //G for gamepad. TODO add to menu. 
        {
            isInDirectMode = !isInDirectMode; //toggle mode
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

    private void ProcessDirectMovement() //ThirdPersonUserControl.cs에서 거의 다 가져오고 조금씩 수정했다. 
    {
        float h = Input.GetAxis("Horizontal");  //ThirdPersonUserControl.cs에서 복사해왔지만 모바일로 만들것은 아니니까 CrossPlatform 어쩌구는 필요없고 그냥 Input으로 대체한다. 참고로 GetAxis 자체에 기본적으로 키보드랑 게임패드 둘 다 들어가있다. 
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        m_Character.Move(m_Move, false, false);
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            switch (cameraRaycaster.layerHit)
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
            m_Character.Move(currentClickTarget - transform.position, false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }
}

