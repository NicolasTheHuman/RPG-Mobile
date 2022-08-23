using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit, ISkill
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
		Debug.Log("Se llamo");

		base.Skill();
		StartCoroutine(Multistrike());
	}

	IEnumerator Multistrike() 
	{
		theBattleSystem.state = BattleState.DAMAGE_CALCULATION;
		skillEnergy = 0;

		List<Unit> deadUnits = new List<Unit>();

		yield return new WaitForSeconds(1f);

		foreach (Unit item in theBattleSystem.enemies)
		{
			bool isDead = item.TakeDamage(damage);

			item.myHUD.SetHP(item.currentHP);

			if (isDead)
			{
				deadUnits.Add(item);
			}
		}

		if (deadUnits.Count > 0)
		{
			for (int i = 0; i < deadUnits.Count; i++)
			{
				currentHP = currentHP + 2;
				maxHP = maxHP + 2;
				theBattleSystem.CheckUnits(deadUnits[i]);
			}
			myHUD.SetHP(currentHP);
		}

		theBattleSystem.state = BattleState.PLAYERTURN;


		theBattleSystem.ChangeTurn();
	}
}
