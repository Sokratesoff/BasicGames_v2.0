using UnityEngine;

public class PingPongGame3D : MonoBehaviour
{
    [SerializeField]private GameObject paddleLeft, paddleRight;
    [SerializeField]private float paddleSpeed = 10f;

    [SerializeField]private Collider wallTop, wallBottom;

    private float paddleHalfHeight;

    void Start(){
        paddleHalfHeight = paddleLeft.GetComponent<Renderer>().bounds.extents.y;
    }

    void Update(){
        #if UNITY_EDITOR || UNITY_STANDALONE
        HandleKeyboardInput();
        #endif

        HandleTouches();
    }

    void HandleTouches(){
        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.GetTouch(i);

            if (touch.position.x < Screen.width / 2f) {
                MovePaddleTouch(paddleLeft, touch);
            } else {
                MovePaddleTouch(paddleRight, touch);
            }
        }
    }

    void MovePaddleTouch(GameObject paddle, Touch touch){
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(ray, out float distance)){
            Vector3 worldPos = ray.GetPoint(distance);
            Vector3 newPos = paddle.transform.position;
            newPos.y = Mathf.MoveTowards(newPos.y, worldPos.y, paddleSpeed * Time.deltaTime);


            // Clamp with paddle size considered
            float minY = wallBottom.bounds.max.y + paddleHalfHeight;
            float maxY = wallTop.bounds.min.y - paddleHalfHeight;
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);


            paddle.transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
        }
    }

    void HandleKeyboardInput(){
        // Left paddle (W/S)
        float moveLeft = 0f;
        if (Input.GetKey(KeyCode.W)) moveLeft = 1f;
        if (Input.GetKey(KeyCode.S)) moveLeft = -1f;
        MovePaddleKeyboard(paddleLeft, moveLeft);

        // Right paddle (Up/Down arrows)
        float moveRight = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) moveRight = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) moveRight = -1f;
        MovePaddleKeyboard(paddleRight, moveRight);
    }

    void MovePaddleKeyboard(GameObject paddle, float direction){
        Vector3 newPos = paddle.transform.position + Vector3.up * direction * paddleSpeed * Time.deltaTime;

        float minY = wallBottom.bounds.max.y + paddleHalfHeight;
        float maxY = wallTop.bounds.min.y - paddleHalfHeight;
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        paddle.transform.position = newPos;
    }
}
