using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager gm;
	public static Transform cam;
	public bool started;
	public float score;
	public static int startAmmo = 5;
	public static int ammoAmount; 

	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI bestScoreText;
	public TextMeshProUGUI completedText;
	public TextMeshProUGUI instructionsText;
	public TextMeshProUGUI recordText;
	public TextMeshProUGUI ammoText;


	private void Awake()
	{
		if (!gm)
		{
			gm = this;
		}

		if (PlayerPrefs.GetFloat("bestScore") == 0)
		{
			bestScoreText.gameObject.SetActive(false);
			PlayerPrefs.SetFloat("bestScore", Mathf.Infinity);
		}

		if (PlayerPrefs.GetFloat("bestScore") == Mathf.Infinity)
		{
			bestScoreText.gameObject.SetActive(false);
		}

		bestScoreText.text = PlayerPrefs.GetFloat("bestScore").ToString("00:00.00");
	}

	public void ResetScore()
	{
		score = 0;
		scoreText.text = score.ToString("00:00.00");
		ammoAmount = startAmmo;
	}

	public void Completed()
	{
		completedText.gameObject.SetActive(true);
		instructionsText.gameObject.SetActive(true);

		if (score < PlayerPrefs.GetFloat("bestScore"))
		{
			PlayerPrefs.SetFloat("bestScore", score);
			bestScoreText.text = PlayerPrefs.GetFloat("bestScore").ToString("00:00.00");
			bestScoreText.gameObject.SetActive(true);
			recordText.gameObject.SetActive(true);
		}
	}

	private void Update()
	{
		if (started)
        {
			score += Time.deltaTime;
			scoreText.text = score.ToString("00:00.00");
			if (ammoAmount > 0)
			{
				ammoText.text = "Ammo " + ammoAmount;
			}
			else if (ammoAmount == 0)
			{
				ammoText.text = "Out of grapple !";
			}
		}
		
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if (Input.GetKeyDown(KeyCode.H))
		{
			completedText.gameObject.SetActive(false);
			instructionsText.gameObject.SetActive(false);
			recordText.gameObject.SetActive(false);
		}
	}
}
