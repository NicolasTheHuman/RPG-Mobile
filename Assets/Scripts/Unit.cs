using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISkill
{
	public string unitName;
	public string SkillName;

	public int damage;
	public int attackEnergy, attackCooldown, skillEnergy, skillCooldown; 

	public int maxHP;
	public int currentHP;

	public Transform battleStation;
	public BattleHUD myHUD;
	public Animator myAnimator;

	[Header("Sound Effects")]
	public AudioClip attackSFX;
	public AudioClip skillSFX;
	public AudioClip takeDamageSFX;

	protected BattleSystem theBattleSystem;

	int damageLimit;

	private void Awake()
	{
		currentHP = maxHP;
		attackEnergy = attackCooldown;
		skillEnergy = 0;
		theBattleSystem = FindObjectOfType<BattleSystem>();
		myAnimator = GetComponent<Animator>();

		damageLimit = damage + 2;
	}

	public virtual bool TakeDamage(int damage)
	{
		currentHP -= damage;
		myAnimator.SetTrigger("Hurt");

		if (currentHP <= 0)
			return true;
		else
			return false;
	}

	public void Heal(int ammount)
	{
		currentHP += ammount;

		if (currentHP > maxHP)
			currentHP = maxHP;
	}

	public virtual void Skill()
	{
		myAnimator.SetTrigger("Skill");
		theBattleSystem.audioManager.PlaySFX(skillSFX);
	}


	public virtual void Skill(Unit enemyToAct)
	{
		StartCoroutine(Buff(enemyToAct));
	}

	IEnumerator Buff(Unit enemyToAct)
	{
		print(enemyToAct.name + " uso su skill en el turno " + theBattleSystem.turn);
		enemyToAct.skillEnergy = 0;

		if (damage < damageLimit)
			damage++;

		currentHP = currentHP + 2;
		if (currentHP > maxHP)
			currentHP = maxHP;

		yield return new WaitForSeconds(1f);
		myHUD.SetHP(currentHP);

		theBattleSystem.ChangeTurn();
	}
}
