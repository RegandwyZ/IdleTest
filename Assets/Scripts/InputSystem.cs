using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour
{
    private const float DRAG_SPEED = 0.03f;
    private const float ZOOM_SPEED = 15f;
    
    private readonly Vector2 _xBounds = new(-40f, 30f);
    private readonly Vector2 _zBounds = new(-30f, 60f);
    private readonly Vector2 _zoomBounds = new(10f, 50f);

    private Vector3 _dragOrigin;
    private bool _isDragging = false;
   

    private void Update()
    {
        if (IsPointerOverUI())
            return;
        
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _dragOrigin = touch.position;
                _isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && _isDragging)
            {
                Vector3 delta = touch.deltaPosition;
                MoveCamera(delta);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _isDragging = false;
                TryInteractWithShop(touch.position);
            }
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            _dragOrigin = Input.mousePosition;
            _isDragging = true;
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector3 delta = Input.mousePosition - _dragOrigin;
            _dragOrigin = Input.mousePosition;
            MoveCamera(delta);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            TryInteractWithShop(Input.mousePosition);
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            ZoomCamera(scroll * ZOOM_SPEED);
        }
        
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            
            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float previousDistance = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
                float currentDistance = (touch1.position - touch2.position).magnitude;
                float zoomDelta = (currentDistance - previousDistance) * 0.01f; 

                ZoomCamera(zoomDelta);
            }
        }
    }

    private void MoveCamera(Vector3 delta)
    {
       
        Vector3 move = new Vector3(-delta.x * DRAG_SPEED, 0, -delta.y * DRAG_SPEED);
        Vector3 moveInWorld = transform.TransformDirection(move);
        moveInWorld.y = 0; 

        transform.position += moveInWorld;
        
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, _xBounds.x, _xBounds.y);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, _zBounds.x, _zBounds.y);
        transform.position = clampedPosition;
    }

    private void ZoomCamera(float delta)
    {
        float newHeight = transform.position.y - delta;
        newHeight = Mathf.Clamp(newHeight, _zoomBounds.x, _zoomBounds.y);

        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }

    private bool TryInteractWithShop(Vector3 screenPosition)
    {
        if (IsPointerOverUI())
            return false;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out var hit))
        {
            var shopView = hit.collider.GetComponent<Shop.ShopView>();
            if (shopView != null)
            {
                shopView.OpenShopPanel();
                return true;
            }
        }
        return false;
    }
    

    private bool IsPointerOverUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return true;
        }

        return false;
    }
}
