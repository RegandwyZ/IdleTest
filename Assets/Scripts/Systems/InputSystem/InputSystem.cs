using Systems.SoundSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems.InputSystem
{
    public class InputSystem : MonoBehaviour
    {
        private const float DRAG_SPEED = 0.03f;
        private const float ZOOM_SPEED = 15f;

        private readonly Vector2 _xBounds = new(-40f, 17f);
        private readonly Vector2 _zBounds = new(-30f, 60f);
        private readonly Vector2 _zoomBounds = new(10f, 50f);

        private Vector3 _dragOrigin;
        private bool _isDragging;
        private bool _isInteractingWithCamera;
        private bool _isShopPanelOpen;

        private void Update()
        {
            if (IsPointerOverUI() || _isShopPanelOpen)
                return;

            HandleMovement();
            HandleZoom();
        }

        private void HandleMovement()
        {
            if (Input.touchCount == 1)
            {
                HandleTouchMovement();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                StartDragging(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0) && _isDragging)
            {
                ContinueDragging(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopDragging(Input.mousePosition);
            }
        }

        private void HandleTouchMovement()
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartDragging(touch.position);
                    break;

                case TouchPhase.Moved:
                    ContinueDragging(touch.position);
                    break;

                case TouchPhase.Ended:
                    StopDragging(touch.position);
                    break;
            }
        }

        private void StartDragging(Vector3 position)
        {
            _dragOrigin = position;
            _isDragging = true;
            _isInteractingWithCamera = false;
        }

        private void ContinueDragging(Vector3 position)
        {
            Vector3 delta = position - _dragOrigin;
            _dragOrigin = position;

            if (delta.sqrMagnitude > 1f)
            {
                MoveCamera(delta);
                _isInteractingWithCamera = true;
            }
        }

        private void StopDragging(Vector3 position)
        {
            _isDragging = false;

            if (!_isInteractingWithCamera && !_isShopPanelOpen)
            {
                TryInteractWithShop(position);
            }
        }

        private void HandleZoom()
        {
            if (Input.touchCount == 2)
            {
                HandleTouchZoom();
            }
            else
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (Mathf.Abs(scroll) > 0.01f)
                {
                    ZoomCamera(scroll * ZOOM_SPEED);
                }
            }
        }

        private void HandleTouchZoom()
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float previousDistance =
                    (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition))
                    .magnitude;
                float currentDistance = (touch1.position - touch2.position).magnitude;
                float zoomDelta = (currentDistance - previousDistance) * 0.01f;

                ZoomCamera(zoomDelta);
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

        private void TryInteractWithShop(Vector3 screenPosition)
        {
            if (IsPointerOverUI() || _isShopPanelOpen) return;

            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var shopView = hit.collider.GetComponent<Shop.ShopView>();
                if (shopView != null)
                {
                    shopView.OpenShopPanel();
                    AudioSystem.Instance.PlaySfx(SfxType.ClickBuilding);
                }
            }
        }

        private bool IsPointerOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                    return true;
            }

            return false;
        }
    }
}