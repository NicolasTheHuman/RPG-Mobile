using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	BattleSystem _bs;
	Billboard _arrowHUD;

	MySceneManager _myScene;
	public Console console;
	SaveAndLoad _saveAndLoad;

	// Start is called before the first frame update
	void Start()
	{
		_bs = FindObjectOfType<BattleSystem>();
		_arrowHUD = FindObjectOfType<Billboard>();
		_myScene = FindObjectOfType<MySceneManager>();
		_saveAndLoad = FindObjectOfType<SaveAndLoad>();

		console.AddCommand("GodMode", GodMode, "Increase your party stats a lot");
		console.AddCommand("WomboCombo", WomboCombo, "1 hot kill on enemies");
		console.AddCommand("Recover", Recover, "Refreshes your party members health");
		console.AddCommand("Recharge", Recharge, "Refreshes skills cooldown");
		console.AddCommand("Restart", Restart, "Restarts the encounter");
		console.AddCommand("Save", Save, "Saves the stats in that moment");
		console.AddCommand("Load", Load, "Loads the progress saved (doesn't revive the dead)");
	}

    // Update is called once per frame
    void Update()
    {
		if(_bs.selectedUnit != null)
			_arrowHUD.transform.position = _bs.selectedUnit.transform.position + (Vector3.up * 2.5f);

		if (Input.GetMouseButtonDown(0) && _bs.state == BattleState.PLAYERTURN || _bs.state == BattleState.ENEMYTURN)
		{

			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit)
			{
				Debug.Log("Hit " + hitInfo.transform.gameObject.name);
				if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Unit"))
				{
					var unitToSelect = hitInfo.transform.gameObject.GetComponent<Unit>(); ;
					if (_bs.partyMembers.Contains(unitToSelect) || _bs.enemies.Contains(unitToSelect))
						_bs.selectedUnit = unitToSelect;
				}
			}
		}
	}

	void GodMode(List<string> data)
	{
		List<Unit> partyMembers = new List<Unit>();
		partyMembers.AddRange(_bs.partyMembers);
		for (int i = 0; i < partyMembers.Count; i++)
		{
			partyMembers[i].maxHP = 99;
			partyMembers[i].currentHP = 99;
			partyMembers[i].myHUD.SetHP(partyMembers[i].currentHP);
			partyMembers[i].damage = 50;
			partyMembers[i].attackCooldown = 0;
			partyMembers[i].skillCooldown = 0;
		}
	}

	void WomboCombo(List<string> data)
	{
		List<Unit> allEnemies = new List<Unit>();
		allEnemies.AddRange(_bs.enemies);
		for (int i = 0; i < allEnemies.Count; i++)
		{
			allEnemies[i].currentHP = 1;
			allEnemies[i].myHUD.SetHP(allEnemies[i].currentHP);
		}

		List<Unit> partyMembers = new List<Unit>();
		partyMembers.AddRange(_bs.partyMembers);
		for (int i = 0; i < partyMembers.Count; i++)
		{
			partyMembers[i].skillCooldown = 0;
		}
	}

	void Recover(List<string> data)
	{
		for (int i = 0; i < _bs.partyMembers.Count; i++)
		{
			_bs.partyMembers[i].currentHP = _bs.partyMembers[i].maxHP;
			_bs.partyMembers[i].myHUD.SetHP(_bs.partyMembers[i].currentHP);
		}
	}

	void Recharge(List<string> data)
	{
		for (int i = 0; i < _bs.partyMembers.Count; i++)
		{
			_bs.partyMembers[i].skillEnergy = _bs.partyMembers[i].skillCooldown;
		}
	}

	void Restart(List<string> data)
	{
		_myScene.Restart();
	}

	void Save(List<string> data)
	{
		_saveAndLoad.SaveData();
	}

	void Load(List<string> data)
	{
		_saveAndLoad.LoadData();
	}
}
