using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { SET_UP, PLAYERTURN, ENEMYTURN, WON, LOST, DAMAGE_CALCULATION }

public class BattleSystem : MonoBehaviour
{
	public BattleState state;

	public int turn;

	public delegate void MyDelegate(Unit unit);

	public List<PlayerBattleHUD> heroesHUD;
	public List<BattleHUD> enemiesHUD;

	public List<Unit> partyMembers = new List<Unit>();
	public List<Unit> enemies = new List<Unit>();

	public List<Unit> deadPartyMembers = new List<Unit>();

	public Unit selectedUnit;
	public GameObject deathPanel;
	WaveManager _waveManager;
	
	int _count = 0;

	public AudioManager audioManager;
	MyAds _adManager;
	MySceneManager _scenes;

	// Start is called before the first frame update
	void Awake()
    {
		_waveManager = FindObjectOfType<WaveManager>().GetComponent<WaveManager>();
		
		_waveManager.wave = 0;
		MyDelegate myDelegate = Attack;

		audioManager = FindObjectOfType<AudioManager>();
		_adManager = FindObjectOfType<MyAds>();
		_scenes = FindObjectOfType<MySceneManager>();
    }

	private void Update()
	{
		if (state == BattleState.SET_UP)
		{
			for (int i = 0; i < partyMembers.Count; i++)
			{
				partyMembers[i].myAnimator.SetBool("Move", true);
			}

		}
		else
		{
			for (int i = 0; i < partyMembers.Count; i++)
			{
				partyMembers[i].myAnimator.SetBool("Move", false);
			}
		}
	}

	private void PlayerTurn()
	{
		Debug.Log("Player Turn");
	}

	public void Attack(Unit unitToAttack)
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerAttack(unitToAttack));
	}

	IEnumerator PlayerAttack(Unit unitToAttack)
	{
		state = BattleState.DAMAGE_CALCULATION;
		unitToAttack.attackEnergy = 0;
		print(unitToAttack.unitName + " attacked & dealt " + unitToAttack.damage + " in the turn " + turn);
		unitToAttack.myAnimator.SetTrigger("Attack");
		audioManager.PlaySFX(unitToAttack.attackSFX);
		yield return new WaitForSeconds(1f);
		audioManager.PlaySFX(selectedUnit.takeDamageSFX);

		bool isDead = selectedUnit.TakeDamage(unitToAttack.damage);
	
		//refresh hp bar
		selectedUnit.myHUD.SetHP(selectedUnit.currentHP);
		state = BattleState.PLAYERTURN;
		if (isDead)
		{
			CheckUnits(selectedUnit);
			ChangeTurn();
		}
		else
		{
			ChangeTurn();
		}
	}

	public void CheckUnits(Unit deadUnit)
	{
		if(enemies.Contains(deadUnit))
		{
			enemies.Remove(deadUnit);
			if(enemies.Count > 0)
			{
				selectedUnit = enemies[0];
				deadUnit.myAnimator.SetBool("Dead", true);
				Destroy(deadUnit.gameObject, 2f);
			}
			else
			{
				print("Mataste a todos genocida, carga la proxima Wave");
				deadUnit.gameObject.SetActive(false);
				_waveManager.wave++;
				if(_waveManager.wave < _waveManager.maxWave)
				{
					state = BattleState.SET_UP;
				}
				else
				{
					Debug.Log("The winner takes it all!");
					state = BattleState.WON;
					EndBattle();
				}
				Destroy(deadUnit, 2f);
			}
		}
		else if(partyMembers.Contains(deadUnit))
		{
			partyMembers.Remove(deadUnit);
			deadUnit.myAnimator.SetBool("Dead", true);
			foreach (var item in partyMembers)
			{
				item.maxHP++;
				item.currentHP++;
				item.myHUD.SetHP(item.currentHP);
				item.attackCooldown--;
				item.attackEnergy = item.attackCooldown;
				item.damage++;
			}

			if(partyMembers.Count == 0)
			{
				state = BattleState.LOST;
				EndBattle();
			}
			else
			{
				state = BattleState.PLAYERTURN;
				PlayerTurn();
			}
		}
	}

	public void ChangeTurn()
	{
		turn++;
		if (state == BattleState.PLAYERTURN)
		{
			state = BattleState.ENEMYTURN;
			print("EnemyTurn = " + turn);
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].skillEnergy++;
			}
			StartCoroutine(EnemyTurn());
		}
		else if (state == BattleState.ENEMYTURN)
		{
			state = BattleState.PLAYERTURN;
			print("PlayerTurn = " + turn);
			for (int i = 0; i < partyMembers.Count; i++)
			{
				partyMembers[i].attackEnergy++;
				partyMembers[i].skillEnergy++;
			}

			PlayerTurn();
		}
	}

	IEnumerator EnemyTurn()
	{

		yield return new WaitForSeconds(0.75f);
		Unit enemyToAct;
		if (_count <= enemies.Count - 1)
		{
			enemyToAct = enemies[_count];
			_count++;
			print("is " + enemyToAct.name + " turn " + turn);
		}
		else
		{
			_count = 0;
			enemyToAct = enemies[_count];
		}
		var heroToAttack = UnityEngine.Random.Range(0, partyMembers.Count);

		if(enemyToAct.skillEnergy >= enemyToAct.skillCooldown) //If they use the skill
		{
			enemyToAct.myAnimator.SetTrigger("Skill");
			audioManager.PlaySFX(enemyToAct.skillSFX);
			enemyToAct.Skill(enemyToAct);
		}
		else // if they attack
		{
			enemyToAct.myAnimator.SetTrigger("Attack");
			audioManager.PlaySFX(enemyToAct.attackSFX);
			yield return new WaitForSeconds(1.5f);
			audioManager.PlaySFX(partyMembers[heroToAttack].takeDamageSFX);

			bool isDead = partyMembers[heroToAttack].TakeDamage(enemyToAct.damage);
			print(enemyToAct.name + " attacked " + partyMembers[heroToAttack].name + " & dealt " + enemyToAct.damage);

			//Refresh HP Bar
			partyMembers[heroToAttack].myHUD.SetHP(partyMembers[heroToAttack].currentHP);

			if (isDead)
			{
				CheckUnits(partyMembers[heroToAttack]);
			}
			else
			{
				ChangeTurn();
			}
		}
		
	}

	private void EndBattle()
	{
		if(state == BattleState.WON)
		{
			Debug.Log("You Won!");
			_scenes.Win();
		}
		else if (state == BattleState.LOST)
		{
			print("Perdiste bro");

			_adManager.currentAdType = AdsType.video;
			_adManager.ShowAds();
			deathPanel.SetActive(true);
		}
	}
}
