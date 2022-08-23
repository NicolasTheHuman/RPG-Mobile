using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
	public delegate void Commands(List<string> data);

	private Dictionary<string, Commands> _console = new Dictionary<string, Commands>();
	private Dictionary<string, string> _description = new Dictionary<string, string>();

	public Text consoleText;
	public InputField consoleInput;

	private int _commandCount = 1;

	// Start is called before the first frame update
	void Awake()
	{
		AddCommand("Commands", CommandsList, "Show available commands.");
		AddCommand("Clear", ClearConsole, "Clears console window.");
	}

	public void AddCommand(string key, Commands command, string description)
	{
		if (_console.ContainsKey(key) == false)
			_console.Add(key, command);
		else
			_console[key] += command;

		if (_description.ContainsKey(key) == false)
			_description.Add(key, description);
		else
			_description[key] += description;
	}

	public void EnterCommand()
	{
		CheckCommand(consoleInput.text);
	}

	private void CheckCommand(string dictionaryKey)
	{

		char[] division = new char[] { ' ' };
		string[] command = dictionaryKey.Split(division);

		string key = command[0];
		List<string> data = new List<string>();
		for (int i = 1; i < command.Length; i++)
		{
			data.Add(command[i]);
		}

		if (_console.ContainsKey(key))
		{
			_console[key](data);
			if(consoleInput.text != "Commands" && consoleInput.text != "Clear")
				consoleText.text = "\n" + "Command Confirmed";
		}
		else consoleText.text += "\n" + "Invalid command. Try again.";

	}

	private void CommandsList(List<string> data)
	{
		consoleText.text += "\n" + "Commands Syntax: CommandName Parameter";
		foreach (var command in _console)
		{
			if (_description[command.Key] == null)
				consoleText.text += "\n" + _commandCount + ": " + command.Key + ".";
			else
				consoleText.text += "\n" + _commandCount + ": " + command.Key + ": " + _description[command.Key];
			_commandCount++;
		}
		_commandCount = 1;
	}

	private void ClearConsole(List<string> data)
	{
		consoleText.text = "Write ´´Commands´´ for available commands. \n";
	}
}
