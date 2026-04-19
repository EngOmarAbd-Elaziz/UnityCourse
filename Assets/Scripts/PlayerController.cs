using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{

    public event EventHandler OnPickedSomething;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Vector3 offset = new Vector3(0, 1f, 0);
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private float interactionCheckTimer = 0f;
    private const float interactionCheckInterval = 0.1f;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;

    }
    // private static PlayerController instance;
    public static PlayerController Instance { get; private set; }
    // {
    //     get { return instance; }
    //     set { instance = value;}
    // }
    // public static PlayerController instanceField;
    // public static PlayerController GetInstanceField()
    // {
    //     return instanceField;
    // }
    // public static void SetInstanceField(PlayerController instanceField)
    // {
    //     PlayerController.instanceField = instanceField;
    // }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one PlayerController instance");
        }
        Instance = this;
    }
    private void Start()

    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }
    private void Update()
    {
        HandleMovement();
        interactionCheckTimer += Time.deltaTime;
        if (interactionCheckTimer >= interactionCheckInterval)
        {
            interactionCheckTimer = 0f;
            HandleInteractions();
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        // use serialized interactDistance so the inspector and gizmo share the same value
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        if (Physics.Raycast(transform.position + offset, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {

                // baseCounter.Interact();
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
            // ClearCounter clearCounter = raycastHit.transform.GetComponent<ClearCounter>();
            // if (clearCounter != null)
            // {

            // }

        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    // Draw the interaction raycast and hit point in the Scene view for debugging
    // private void OnDrawGizmos()
    // {
    //     // Simple: draw the ray used for counter detection and mark hits.
    //     // Use lastInteractDir if available; otherwise fall back to transform.forward so the ray is always visible.
    //     Vector3 dir = lastInteractDir != Vector3.zero ? lastInteractDir : transform.forward;
    //     Vector3 start = transform.position + offset;
    //     Vector3 end = start + dir.normalized * interactDistance;

    //     if (Physics.Raycast(start, dir, out RaycastHit hitInfo, interactDistance, countersLayerMask))
    //     {
    //         Gizmos.color = Color.green;
    //         Gizmos.DrawLine(start, hitInfo.point);
    //         Gizmos.DrawSphere(hitInfo.point, 0.08f);
    //     }
    //     else
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawLine(start, end);
    //     }
    // }

    private void HandleMovement()
    {


        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRedius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRedius, moveDir, moveDistance);

        if (canMove)
        {
            transform.position += moveDir * moveDistance;

        }
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRedius, moveDirX, moveDistance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRedius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {

                }
            }
        }
        isWalking = moveDir != Vector3.zero;

        float rotationSpeed = 10f;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {

        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
