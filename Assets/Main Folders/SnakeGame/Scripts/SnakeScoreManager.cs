using UnityEngine;
using TMPro;

public class SnakeScoreManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI scoreText;
    [SerializeField]private float score;

    public void IncreaseTheScore(){
        score++;
        scoreText.text = score.ToString();
    }

    public void ResetTheScore(){
        score = 0;
        scoreText.text = score.ToString();
    }

}
