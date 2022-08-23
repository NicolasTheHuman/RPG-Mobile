using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WaveManager : MonoBehaviour
{
	public int wave;
	public int maxWave;
	public List<GameObject> heroesClasses;
	public List<GameObject> enemyTypes;

	public List<Transform> heroesBattleStations;
	public List<Transform> enemiesBattleStations;

	public Boss boss;

	BattleSystem theBattleSystem;
	List<GameObject> _enemyUnitTypes = new List<GameObject>();

	[Header("Stages")]
	public Unit[] stage1 = new Unit[3];
	public Unit[] stage2 = new Unit[3];
	public Unit[] stage3 = new Unit[3];

	private void Awake()
	{
		wave = 0;
		
		theBattleSystem = FindObjectOfType<BattleSystem>();
		SetupBattle();
	}

	private void Start()
	{
		for (int i = 0; i < enemyTypes.Count; i++)
		{
			_enemyUnitTypes.Add(enemyTypes[i]);
		}

		_enemyUnitTypes.Insert(1, boss.gameObject);
	}


	// Update is called once per frame
	void Update()
	{
		if (theBattleSystem.state == BattleState.SET_UP && wave < maxWave)
		{
			EnemySetup();
		}
	}

	public void SetupBattle()
	{
		PartySetup();

		EnemySetup();
	}

	void PartySetup()
	{
		for (int i = 0; i < heroesBattleStations.Count; i++)
		{
			var heroToSpawn = Instantiate(heroesClasses[i], heroesBattleStations[i].position, heroesClasses[i].transform.rotation);
			var unitToAdd = heroToSpawn.GetComponent<Unit>();
			theBattleSystem.partyMembers.Add(unitToAdd);
			theBattleSystem.deadPartyMembers.Add(unitToAdd);

			unitToAdd.battleStation = heroesBattleStations[i];
			unitToAdd.myHUD = theBattleSystem.heroesHUD[i];
			theBattleSystem.heroesHUD[i].myUnit = unitToAdd;
			theBattleSystem.heroesHUD[i].SetHUD(unitToAdd);
		}
	}

	void EnemySetup()
	{
		for (int i = 0; i < enemiesBattleStations.Count; i++)
		{
			Unit spawnedEnemy = null;
			switch (wave)
			{
				case 0:
					if (stage1[i] == null)
						continue;
					spawnedEnemy = Instantiate(stage1[i], enemiesBattleStations[i].position, new Quaternion(0, 180, 0, 0));
					break;
				case 1:
					if (stage2[i] == null)
						continue;
					spawnedEnemy = Instantiate(stage2[i], enemiesBattleStations[i].position, new Quaternion(0, 180, 0, 0));
					break;
				case 2:
					if (stage3[i] == null)
						continue;
					spawnedEnemy = Instantiate(stage3[i], enemiesBattleStations[i].position, new Quaternion(0, 180, 0, 0));
					break;
			}

			if(spawnedEnemy != null)
			{
				theBattleSystem.enemies.Add(spawnedEnemy);

				spawnedEnemy.battleStation = enemiesBattleStations[i];
				spawnedEnemy.myHUD = theBattleSystem.enemiesHUD[i];
				theBattleSystem.enemiesHUD[i].myUnit = spawnedEnemy;
				theBattleSystem.enemiesHUD[i].SetHUD(spawnedEnemy);
			}
			
		}

		theBattleSystem.selectedUnit = theBattleSystem.enemies[1];

		theBattleSystem.state = BattleState.ENEMYTURN;
		theBattleSystem.ChangeTurn();
	}

	public void Continue()
	{

		for (int i = 0; i < heroesBattleStations.Count; i++)
		{
			var unitToRevive = theBattleSystem.deadPartyMembers[i];
			theBattleSystem.partyMembers.Add(unitToRevive);
			unitToRevive.myAnimator.SetBool("Dead", false);
			unitToRevive.currentHP = unitToRevive.maxHP;

			unitToRevive.myHUD = theBattleSystem.heroesHUD[i];
			theBattleSystem.heroesHUD[i].myUnit = unitToRevive;
			theBattleSystem.heroesHUD[i].SetHUD(unitToRevive);
			unitToRevive.myHUD.SetHP(unitToRevive.currentHP);
		}

		//Removes & destroys all enemies for a restart of the wave
		foreach (var Unit in theBattleSystem.enemies)
		{
			Unit.gameObject.SetActive(false);
			Destroy(Unit, 1.5f);
		}

		theBattleSystem.enemies.Clear();

		EnemySetup();
	}
}
