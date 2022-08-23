using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Unit, ISkill
{
	private void Start()
	{
		skillEnergy = skillCooldown;
	}

	public override void Skill()
	{
		base.Skill();
		StartCoroutine(Heal());
	}

	IEnumerator Heal()
	{
		theBattleSystem.state = BattleState.DAMAGE_CALCULATION;
		skillEnergy = 0;
		if (theBattleSystem.selectedUnit.currentHP > 0)
		{
			theBattleSystem.selectedUnit.Heal(4);
			Heal(2);
		}
		else
			Heal(6);

		yield return new WaitForSeconds(1.5f);

		theBattleSystem.state = BattleState.PLAYERTURN;

		//refresh hp bar
		theBattleSystem.selectedUnit.myHUD.SetHP(theBattleSystem.selectedUnit.currentHP);
		myHUD.SetHP(currentHP);

		theBattleSystem.ChangeTurn();
	}
}
