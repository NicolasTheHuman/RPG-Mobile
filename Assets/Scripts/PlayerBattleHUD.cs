using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBattleHUD : BattleHUD
{
	public Button attackButton;
	public Button skillButton;

	BattleSystem theBattleSystem;
	
    // Start is called before the first frame update
    void Start()
    {
		theBattleSystem = FindObjectOfType<BattleSystem>();

		var skillText = skillButton.GetComponentInChildren<TextMeshProUGUI>();
		skillText.text = myUnit.SkillName;

		attackButton.onClick.AddListener(delegate { theBattleSystem.Attack(myUnit); });
		attackButton.onClick.AddListener(Deactivate);
		attackButton.onClick.AddListener(delegate { Deactivate(); });
		skillButton.onClick.AddListener(myUnit.Skill);
		skillButton.onClick.AddListener(Deactivate);
		skillButton.onClick.AddListener(delegate { Deactivate(); });
    }

    // Update is called once per frame
    void Update()
    {
		if (theBattleSystem.state != BattleState.PLAYERTURN)
		{
			Deactivate();
		}
		else
		{
			if (myUnit.attackEnergy >= myUnit.attackCooldown)
				ButtonState(attackButton, true);

			if (myUnit.skillEnergy >= myUnit.skillCooldown)
				ButtonState(skillButton, true);
		}

		/*if (myUnit.attackEnergy < myUnit.attackCooldown)
			ButtonState(attackButton, false);
		else if (myUnit.skillEnergy < myUnit.skillCooldown)
			ButtonState(skillButton, false);*/

		if (myUnit.currentHP <= 0)
			Deactivate();
	}

	public void Deactivate()
	{
		attackButton.interactable = false;
		skillButton.interactable = false;
	}

	void ButtonState(Button button, bool interactable)
	{
		button.interactable = interactable;
	}
}
