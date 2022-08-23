using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Unit, ISkill
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override bool TakeDamage(int damage)
	{
		this.damage++;
		return base.TakeDamage(damage);
	}

	public override void Skill(Unit enemyToAct)
	{
		myAnimator.SetTrigger("Skill");
		StartCoroutine(Megahit(enemyToAct));
	}

	IEnumerator Megahit(Unit enemyToAct)
	{
		skillEnergy = 0;

		List<Unit> deadUnits = new List<Unit>();

		yield return new WaitForSeconds(2f);

		foreach (Unit item in theBattleSystem.partyMembers)
		{
			bool isDead = item.TakeDamage(damage);
			
			item.myHUD.SetHP(item.currentHP);

			
			if (isDead)
			{
				deadUnits.Add(item);
			}
		}

		yield return new WaitForSeconds(0.5f);

		if (deadUnits.Count > 0)
		{
			for (int i = 0; i < deadUnits.Count; i++)
			{
				theBattleSystem.CheckUnits(deadUnits[i]);
			}
		}
		else
		{
			theBattleSystem.ChangeTurn();
		}

	}
}
