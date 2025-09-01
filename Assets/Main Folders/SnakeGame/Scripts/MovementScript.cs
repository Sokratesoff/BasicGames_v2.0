using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour
{
    public SnakeScoreManager scoreManager;
    public float timer = 0f;
    public float moveInterval = 1f; // Time between moves
    public Vector3 lastPos;
    public GameObject newTailPrefab, fruit, parentOfTails;
    public Vector2 xRange = new(-10f, 10f);
    public Vector2 zRange = new(-10f, 10f);
    public float yPosition = 1f;
    private Vector3 moveDirection = Vector3.left;

    private bool ableToChangeDirection = true;

    private Color rayColor;

    private Vector2 swipeStart;
    private Vector2 swipeEnd;
    private float minSwipeDistance = 50f;

    private void Start(){
        TailHandler.tails ??= new List<GameObject>();
        TailHandler.tails.Clear();
    }

    private void Update(){
        Debug.DrawRay(transform.position, new(0f, -5f, 0f), rayColor);

        HandleInput();

        HandleSwipeInput();

        timer += Time.deltaTime;

        if (timer >= moveInterval){
            timer = 0f;
            MoveHeadOfSnake();
        }
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("SnakeTail")){
            SceneManager.LoadScene("SnakeGame");
        }

        if(!collision.gameObject.CompareTag("Fruit")) return;

        scoreManager.IncreaseTheScore();

        Destroy(collision.gameObject);
        AddNewTail();
    }

    private void AddNewTail(){
        float randomX = Random.Range(xRange.x, xRange.y);
        float randomZ = Random.Range(zRange.x, zRange.y);
        Vector3 spawnPosition = new(randomX, yPosition, randomZ);

        Instantiate(fruit, spawnPosition, Quaternion.identity);

        GameObject newTail = Instantiate(newTailPrefab, lastPos, Quaternion.identity, parentOfTails.transform);
        newTail.name = "tail";
        TailHandler.tails.Add(newTail);
    }

    private void HandleInput(){
        if(!ableToChangeDirection)
            return;

        if (Input.GetKey(KeyCode.W)){
            moveDirection = Vector3.forward;
        }else if (Input.GetKey(KeyCode.S)){
            moveDirection = Vector3.back;
        }else if (Input.GetKey(KeyCode.A)){
            moveDirection = Vector3.left;
        }else if (Input.GetKey(KeyCode.D)){
            moveDirection = Vector3.right;
        }
    }

    private void HandleSwipeInput()
    {
        if (Input.touchCount == 0) return;


        Touch touch = Input.GetTouch(0);


        if (touch.phase == TouchPhase.Began)
        {
            swipeStart = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            swipeEnd = touch.position;
            DetectSwipe();
        }
    }

    private void DetectSwipe()
    {
        Vector2 swipe = swipeEnd - swipeStart;


        if (swipe.magnitude < minSwipeDistance)
            return; // Ignore small swipes


        bool isHorizontal = Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y);


        if (isHorizontal)
        {
            moveDirection = swipe.x > 0 ? Vector3.right : Vector3.left;
        }
        else
        {
            moveDirection = swipe.y > 0 ? Vector3.forward : Vector3.back;
        }
    }

    private void MoveHeadOfSnake(){
        //if (!canMove) return;

        lastPos = transform.position;
        transform.position += moveDirection;

        CheckForEdges();
        
        TailHandler.MoveTails();
        return;
    }

    private void CheckForEdges(){
        Ray ray = new(gameObject.transform.position, -transform.up);

        if(Physics.Raycast(ray, out RaycastHit hit, 2f)){
            if(hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Fruit") || hit.collider.name == "tail"){ 
                ableToChangeDirection = true;
                rayColor = Color.green; 
                return; 
            }

            MoveToTheOtherSide();
        }else{
            MoveToTheOtherSide();
        }
    }

    private void MoveToTheOtherSide(){
        ableToChangeDirection = false;

        rayColor = Color.red;

        Vector3 direction = transform.position - lastPos;
        Vector3 normalizedDirection = direction.normalized;

        if (Mathf.Abs(normalizedDirection.x) == 1f || Mathf.Abs(normalizedDirection.x) == -1f)
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        else if (Mathf.Abs(normalizedDirection.z) == 1f || Mathf.Abs(normalizedDirection.y) == -1f)
            transform.position = new Vector3(transform.position.x, transform.position.y, -transform.position.z);
    }
}