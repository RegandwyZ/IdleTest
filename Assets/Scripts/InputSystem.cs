using UnityEngine;
using UnityEngine.EventSystems;

public class InputSystem : MonoBehaviour
{
    private readonly float _dragSpeed = 0.2f;
    private Vector2 _xBounds = new(-100f, 100f);
    private Vector2 _zBounds = new(-100f, 100f);

    private Vector3 _dragOrigin;
    private bool _isDragging = false;

    private void Update()
    {
        if (IsPointerOverUI())
            return;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _dragOrigin = touch.position;
                _isDragging = true;
                TryInteractWithShop(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && _isDragging)
            {
                Vector3 delta = touch.deltaPosition;
                MoveCamera(delta);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                _isDragging = false;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            _dragOrigin = Input.mousePosition;
            _isDragging = true;
            TryInteractWithShop(Input.mousePosition);
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
        }
    }

    private void MoveCamera(Vector3 delta)
    {
        Vector3 move = new Vector3(-delta.x * _dragSpeed, 0, -delta.y * _dragSpeed);
        transform.position += move;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, _xBounds.x, _xBounds.y),
            transform.position.y,
            Mathf.Clamp(transform.position.z, _zBounds.x, _zBounds.y)
        );
    }

    private void TryInteractWithShop(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var shopView = hit.collider.GetComponent<Shop.ShopView>();
            if (shopView != null)
            {
                shopView.OpenShopPanel();
            }
        }
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