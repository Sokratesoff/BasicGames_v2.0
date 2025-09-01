using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallMovement : MonoBehaviour
{
    [SerializeField]private float ballSpeed = 8f, minSpeed = 5f, paddleInfluence = 2f;
    [SerializeField]private ScoreManager scoreManager;

    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;
        LaunchBall();
    }

    void FixedUpdate(){
        ballSpeed += Time.deltaTime;
        MaintainSpeed();
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Paddle")) {
            ballSpeed += 0.35f;
            BounceOffPaddle(collision);
        }
        else if (collision.gameObject.CompareTag("Wall")) {
            ballSpeed += 0.35f;
            BounceOffWall(collision);
        }

        if (!collision.gameObject.CompareTag("ScoreWall"))
            return;

        if(transform.position.x < 0){
            scoreManager.IncreaseTheLeftScore();
        }else if(transform.position.x > 0){
            scoreManager.IncreaseTheRightScore();
        }

        ballSpeed = 8f;
        
        transform.position = new(0f, 0f, -0.3f);
        LaunchBall();
    }

    public void LaunchBall(){
        float dirX = Random.value < 0.5f ? -1f : 1f;
        float dirY = Random.Range(-0.5f, 0.5f);
        Vector3 direction = new Vector3(dirX, dirY, 0).normalized;
        rb.linearVelocity = direction * ballSpeed;
    }

    private void MaintainSpeed(){
        if (rb.linearVelocity.sqrMagnitude < 0.01f) {
            LaunchBall();
        }
        else {
            rb.linearVelocity = rb.linearVelocity.normalized * Mathf.Max(minSpeed, ballSpeed);
        }
    }

    private void BounceOffPaddle(Collision collision){
        // Calculate hit factor (where on the paddle it hit)
        float y = HitFactor(transform.position, collision.transform.position, collision.collider.bounds.size.y);

        // Always reverse X when hitting paddle
        float dirX = rb.linearVelocity.x > 0 ? 1f : -1f;
        dirX = -dirX;

        // Add paddle movement influence
        Rigidbody paddleRb = collision.rigidbody;
        float paddleVelY = paddleRb != null ? paddleRb.linearVelocity.y : 0f;

        Vector3 dir = new Vector3(dirX, y, 0) + new Vector3(0, paddleVelY * paddleInfluence, 0);
        dir = dir.normalized;

        rb.linearVelocity = dir * ballSpeed;
    }

    private void BounceOffWall(Collision collision){
        Vector3 reflectDir = Vector3.Reflect(rb.linearVelocity.normalized, collision.contacts[0].normal);
        Vector3 sharpDir = (reflectDir + collision.contacts[0].normal * 0.5f).normalized;
        rb.linearVelocity = sharpDir * ballSpeed * 1.5f;
    }

    private float HitFactor(Vector3 ballPos, Vector3 paddlePos, float paddleHeight){
        return (ballPos.y - paddlePos.y) / paddleHeight;
    }
}
