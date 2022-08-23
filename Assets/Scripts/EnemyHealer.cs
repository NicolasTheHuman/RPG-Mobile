using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealer : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		
    }

	public override void Skill(Unit EnemyToAct)
	{
		myAnimator.SetTrigger("Skill");
		StartCoroutine(HealAll(EnemyToAct));
	}

	IEnumerator HealAll(Unit EnemyToAct)
	{
		Debug.Log(EnemyToAct.name + " uso su skill en el turno " + theBattleSystem.turn);
		yield return new WaitForSeconds(1.5f);
		for (int i = 0; i < theBattleSystem.enemies.Count; i++)
		{
			theBattleSystem.enemies[i].Heal(4);
			theBattleSystem.enemies[i].myHUD.SetHP(theBattleSystem.enemies[i].currentHP);
		}

		skillEnergy = 0;

		theBattleSystem.ChangeTurn();
	}
}
