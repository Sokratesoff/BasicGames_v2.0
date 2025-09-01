using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isGameStarted = false;
    public float jumpForce = 5f;
    [SerializeField]private float tubeSpeed;
    [SerializeField]private GameObject startInfoGameObject;
    [SerializeField]private FlappyBirdScoreManager scoreManager;

	void Awake(){
		tubeSpeed = TubeManager.script.tubeSpeed;
	}

	void Update(){
        if(!Input.GetMouseButtonDown(0) && !Input.GetKeyDown(KeyCode.Space)) 
            return;
        
        GetComponent<Rigidbody>().linearVelocity = new Vector3(GetComponent<Rigidbody>().linearVelocity.x, 0f, GetComponent<Rigidbody>().linearVelocity.z);
        GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        if(isGameStarted) return;

        isGameStarted = true;
		StartCoroutine(TubeManager.script.HandleCreationOfTubes());
        startInfoGameObject.SetActive(false);
	}

	void OnCollisionEnter(Collision collision){
		if(!collision.gameObject.CompareTag("Tube")) return;

        scoreManager.ResetTheScore();

        TubeManager.script.StopAllCoroutines();
            
        for(int i = 0; i < TubeManager.script.createdTubes.Count; i++)
            Destroy(TubeManager.script.createdTubes[i]);

        transform.position = new(transform.position.x, 1f, transform.position.z);

        TubeManager.script.tubeSpeed = tubeSpeed;

        TubeManager.script.tubeDistance = 0f;

        isGameStarted = false;

        startInfoGameObject.SetActive(true);
	}
}
