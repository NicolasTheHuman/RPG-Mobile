using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TutorialManager : MonoBehaviour
{
	public int numberOfTouches = 0;
	public GameObject boxA;
	public TextMeshProUGUI textBoxA;

	public GameObject boxB;
	public TextMeshProUGUI textBoxB;

	public GameObject abilities;
	BattleSystem _bs;

	private void Awake()
	{
		_bs = FindObjectOfType<BattleSystem>();
	}

	// Start is called before the first frame update
	void Start()
    {
		abilities.SetActive(false);
		_bs.enemies[0].currentHP = 3;
		_bs.enemies[0].myHUD.SetHP(3);
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0))
			numberOfTouches++;

		OnTouch();
    }

	void OnTouch()
	{
		switch (numberOfTouches)
		{
			case 1:
				textBoxB.text = "Check your abilities!!";
				break;
			case 2:
				abilities.SetActive(true);
				boxA.SetActive(false);
				boxB.SetActive(false);
				break;
			case 3:
				abilities.SetActive(false);
				boxA.SetActive(true);
				boxB.SetActive(true);
				textBoxB.text = "Try targetting the enemy with low health";
				break;
			case 4:
				textBoxB.text = "Use Deathblow or Multi attack to kill the target";
				break;
			case 5:
				boxA.SetActive(false);
				break;
			case 6:
				textBoxB.text = "Remember to target the enemy before an attack and an ally before a heal";
				break;
			case 7:
				boxB.SetActive(false);
				break;
		}
	}
}
