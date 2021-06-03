using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AddingScoreToUI : MonoBehaviour
{
    private void Update()
    {
        this.GetComponent<Text>().text = scores.playerScore.ToString();
    }
}
