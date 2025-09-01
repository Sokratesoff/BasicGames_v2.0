using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI leftScoreText, rightScoreText;
    [SerializeField]private float leftPlayerScore = 0, rightPlayerScore = 0;

    public void IncreaseTheLeftScore(){
        leftPlayerScore++;
        leftScoreText.text = leftPlayerScore.ToString();
    }

    public void IncreaseTheRightScore(){
        rightPlayerScore++;
        rightScoreText.text = rightPlayerScore.ToString();
    }
}
