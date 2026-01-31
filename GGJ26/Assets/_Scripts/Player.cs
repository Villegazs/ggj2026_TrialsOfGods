using System;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    
    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    private bool isWalking;
    private Vector3 lastInteractDir;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one Player in the scene!");
        }
        Instance = this;
    }

    private void Start()
    {
        //gameInput.OnInteractAction += GameInput_OnInteractAction;
        //gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }
    private void Update()
    {
        HandleMovement();
        //HandleInteractions();
    }
    

    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up* playerHeight, playerRadius, moveDir, moveDistance); //Check if there is a wall in front of the player

        if (!canMove)
        {
            //Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Can move only on the x
                moveDir = moveDirX;
            }
            else
            {
                // Cannot move only on the x
                
                //Attempt only z movement
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    // Can move only on the z
                    moveDir = moveDirZ;
                }
                else
                {
                    //Cannot move in any direction
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        Debug.Log(moveDir);
        Debug.Log($"Can move: {canMove}");
        
        isWalking = moveDir != Vector3.zero;
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        
        //Debug.Log(inputVector);
    }
}
