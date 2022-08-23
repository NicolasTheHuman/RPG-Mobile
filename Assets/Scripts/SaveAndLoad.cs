using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class MyValues
{
	public List<string> unitName = new List<string>();
	public List<int> damage = new List<int>();
	public List<int> attackEnergy = new List<int>();
	public List<int> attackCooldown = new List<int>();
	public List<int> skillEnergy = new List<int>();
	public List<int> skillCooldown = new List<int>();

	public List<int> maxHP = new List<int>();
	public List<int> currentHP = new List<int>();
}

public class SaveAndLoad : MonoBehaviour
{
	MyValues myValues;
	List<Unit> unitsToSave;
	string _path;
	string _data;

	WaveManager _waveManager;
	BattleSystem _bs;

	private void Awake()
	{
		_waveManager = FindObjectOfType<WaveManager>();
		_bs = FindObjectOfType<BattleSystem>();

		//Windows
		//_path = Application.streamingAssetsPath + "/SaveData.json";

		//Android
		_path = Application.persistentDataPath + "/SaveData.json";
		myValues = LoadFile();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.F11))
			SaveData();

		if (Input.GetKeyDown(KeyCode.F12))
			LoadData();
    }

	public void SaveData()
	{
		ClearDataFromLists();

		Debug.Log("Saving...");
		unitsToSave = FindObjectsOfType<Unit>().ToList();
		foreach (var item in unitsToSave)
		{
			myValues.unitName.Add(item.unitName);
			myValues.damage.Add(item.damage);
			myValues.attackEnergy.Add(item.attackEnergy);
			myValues.attackCooldown.Add(item.attackCooldown);
			myValues.skillEnergy.Add(item.skillEnergy);
			myValues.skillCooldown.Add(item.skillCooldown);

			myValues.maxHP.Add(item.maxHP);
			myValues.currentHP.Add(item.currentHP);
		}

		SaveFile();
	}

	void ClearDataFromLists()
	{
		myValues.unitName.Clear();
		myValues.damage.Clear();
		myValues.attackEnergy.Clear();
		myValues.attackCooldown.Clear();
		myValues.skillEnergy.Clear();
		myValues.skillCooldown.Clear();

		myValues.maxHP.Clear();
		myValues.currentHP.Clear();

	}

	public void LoadData()
	{
		Debug.Log("Loading...");
		myValues = LoadFile();

		List<Unit> unitsAlive = FindObjectsOfType<Unit>().ToList();
		//unitsToSave = FindObjectsOfType<Unit>().ToList();
		for (int i = 0; i < unitsToSave.Count; i++)
		{
			if (!unitsAlive.Contains(unitsToSave[i]))
			{
				Debug.Log("Skipping...");
				continue;
			}

			unitsToSave[i].unitName = myValues.unitName[i];
			unitsToSave[i].damage = myValues.damage[i];
			unitsToSave[i].attackEnergy = myValues.attackEnergy[i];
			unitsToSave[i].attackCooldown = myValues.attackCooldown[i];
			unitsToSave[i].skillEnergy = myValues.skillEnergy[i];
			unitsToSave[i].skillCooldown = myValues.skillCooldown[i];

			unitsToSave[i].maxHP = myValues.maxHP[i];
			unitsToSave[i].currentHP = myValues.currentHP[i];

			unitsToSave[i].myHUD.SetHP(unitsToSave[i].currentHP);
		}
	}

	MyValues LoadFile()
	{
		if (!File.Exists(_path))
			SaveFile();

		_data = File.ReadAllText(_path);
		return JsonUtility.FromJson<MyValues>(_data);
	}

	void SaveFile()
	{
		if (myValues == null)
			myValues = new MyValues();

		StreamWriter file = File.CreateText(_path);
		string json = JsonUtility.ToJson(myValues, true);

		file.Write(json);
		file.Close();
	}
}
