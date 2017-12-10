using UnityEngine;

[RequireComponent (typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour
{
    [SerializeField] private Texture2D walkCursor = null;
    [SerializeField] private Texture2D unkownCursor = null;
    [SerializeField] private Texture2D targetCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);   //핫스팟은 커서 아이콘의 어느 지점이 실제 클릭을 담당할 지 결정하는 것. 따라서 저 벡터는 커서 아이콘을 보고 직접 결정하면 되는데, (96, 96)은 벤이 결정한 것. 하지만 화살표 max size를 32로 바꾼 후에 필요없어짐. 

    private CameraRaycaster cameraRaycaster;

	void Start ()
	{
	    cameraRaycaster = GetComponent<CameraRaycaster>();
	    cameraRaycaster.onLayerChange += OnLayerChanged;   //registering  TODO Consider de-registering OnLayerChange on leaving all game scenes.
	}


    //그냥 Update()로 하면 게임 시작 시에 default로 되어서 로그에러가 발생한다. 그거 없애주려고 LateUpdate()로 바꾼 것. 
    //델리게이트로 바꾼다!
    void OnLayerChanged(Layer newLayer)
    {
        print("Cursor over new layer");
	    switch (newLayer)
	    {
            case Layer.Walkable:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
	            Cursor.SetCursor(unkownCursor, cursorHotspot, CursorMode.Auto);
	            break;
	        case Layer.Enemy:
	            Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
	            break;
            default:
                Debug.LogError("Don't know what cursor to show");
                return;

        }		
	}
}
