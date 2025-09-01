using UnityEngine;

public class Tube : MonoBehaviour
{
	private FlappyBirdScoreManager scoreManager;

	void Start() {
		scoreManager = GameObject.Find("Score Manager").GetComponent<FlappyBirdScoreManager>();	
	}

	void FixedUpdate(){
		if(transform.position.x <= TubeManager.script.endPosition.x)
			transform.position = new(
				TubeManager.script.startPosition.x, 
				Random.Range(TubeManager.script.minTubeHeight, TubeManager.script.maxTubeHeight), 
				TubeManager.script.startPosition.z
			);

		transform.Translate(TubeManager.script.tubeSpeed * Vector3.left.normalized);
	}

	void OnTriggerEnter(Collider collider) {
		if(collider.CompareTag("FlappyBird")){
            scoreManager.IncreaseTheScore();
        }	
	}
}
