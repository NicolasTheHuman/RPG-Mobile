using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : Unit, ISkill
{


	// Start is called before the first frame update
	private void Start()
	{
		skillEnergy = skillCooldown;
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public override void Skill()
	{
		base.Skill();
		StartCoroutine(DeathBlow());
	}

	IEnumerator DeathBlow()
	{
		theBattleSystem.state = BattleState.DAMAGE_CALCULATION;
		skillEnergy = 0;
		bool isDead = theBattleSystem.selectedUnit.TakeDamage(damage);

		yield return new WaitForSeconds(1f);

		//refresh hp bar
		theBattleSystem.selectedUnit.myHUD.SetHP(theBattleSystem.selectedUnit.currentHP);
		if (isDead)
		{
			attackEnergy++;
			skillEnergy++;
			theBattleSystem.state = BattleState.PLAYERTURN;
			theBattleSystem.CheckUnits(theBattleSystem.selectedUnit);
		}
		else
		{
			theBattleSystem.state = BattleState.PLAYERTURN;
			theBattleSystem.ChangeTurn();
		}
	}
}
