using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class TextEditor : IOriginator {
	private UndoableTextFile _currentFile;
	private Caretaker _caretaker = new Caretaker(); 
	private bool isFileOpened = false; 

	public void OpenFile(string filePath) {
		_currentFile = new UndoableTextFile(filePath);
		_currentFile.Load();
		isFileOpened = true;
		_caretaker.Clear();
		_caretaker.SaveState(this);
		Console.WriteLine("The file is open. Current content: \n" + _currentFile.Content);
	}

	public void EditFile() {
		if (!isFileOpened) {
			Console.WriteLine("\nOpen the file first!");
			return;
		}

		Console.WriteLine("Enter a new text (write at the end 'SAVE'):");
		List<string> lines = new List<string>();

		string line;
		while ((line = Console.ReadLine()) != "SAVE") {
			lines.Add(line);
		}

		string newContent = string.Join("\n", lines);
		_caretaker.SaveState(this); 
		_currentFile.ChangeContent(newContent);
		Console.WriteLine("The text has been changed!");
	}

	public void SerializeBinary(string filePath) {
		try {
			using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, _currentFile);
				Console.WriteLine($"Binary data saved: {filePath}");
			}
		} catch (Exception ex) {
			Console.WriteLine($"Error saving binary file: {ex.Message}");
		}
	}

	public void DeserializeBinary(string filePath) {
		try {
			using (FileStream stream = new FileStream(filePath, FileMode.Open)) {
				BinaryFormatter formatter = new BinaryFormatter();
				UndoableTextFile tempFile = (UndoableTextFile)formatter.Deserialize(stream);

				_currentFile = tempFile;
				_currentFile.ClearHistory(); 

				Console.WriteLine($"Binary file loaded: {filePath}");
				Console.WriteLine($"Current content:\n{_currentFile.Content}");
			}
		} catch (Exception ex) {
			Console.WriteLine($"Error loading binary file: {ex.Message}");
		}
	}

	public void Undo() {
		if (!isFileOpened) {
			Console.WriteLine("\nOpen the file");
			return;
		}

		if (_caretaker.CanUndo()) {
			_caretaker.RestoreState(this);
			object currentState = _caretaker.GetCurrentState();

			if (currentState is TextFileMemento memento) {
				Console.WriteLine($"\nUndo to version from {memento.Timestamp}");
			} else {
				Console.WriteLine("\nUndo completed, but timestamp unavailable");
			}
		} else {
			Console.WriteLine("\nNo changes to undo");
		}
	}

	public void SearchByKeywords() {
		FileIndexer indexer = new FileIndexer();

		Console.Write("Enter the path to the folder to search for: ");
		string directory = Console.ReadLine();

		Console.Write("Enter the search keywords (separated by spaces): ");
		string input = Console.ReadLine();
		List<string> keywords = input.Split(' ').ToList();

		try {
			Dictionary<string, List<string>> index = indexer.CreateIndex(directory, keywords);
			indexer.DisplayIndex(index);
		} catch (Exception ex) {
			Console.WriteLine($"An error occurred during the search: {ex.Message}");
		}
	}

	public void SerializeXml(string filePath) {
		try {
			using (TextWriter writer = new StreamWriter(filePath)) {
				XmlSerializer serializer = new XmlSerializer(typeof(UndoableTextFile));
				serializer.Serialize(writer, _currentFile);
				Console.WriteLine($"XML data saved: {filePath}");
			}
		} catch (Exception ex) {
			Console.WriteLine($"Error saving XML file: {ex.Message}");
		}
	}

	public void DeserializeXml(string filePath) {
		try {
			using (TextReader reader = new StreamReader(filePath)) {
				XmlSerializer serializer = new XmlSerializer(typeof(UndoableTextFile));
				_currentFile = (UndoableTextFile)serializer.Deserialize(reader);
				_currentFile.ClearHistory();

				Console.WriteLine($"XML file loaded: {filePath}");
				Console.WriteLine($"Current content:\n{_currentFile.Content}");
			}
		} catch (Exception ex) {
			Console.WriteLine($"Error loading XML file: {ex.Message}");
		}
	}

	public void SaveFile() {
		_currentFile.Save();
		Console.WriteLine("The file is saved!");
	}

	object IOriginator.GetMemento() {
		return new TextFileMemento(_currentFile.Content);
	}

	void IOriginator.SetMemento(object memento) {
		if (memento is TextFileMemento textMemento) {
			_currentFile.ChangeContent(textMemento.SavedContent);
		} else {
			throw new ArgumentException("Invalid memento type.");
		}
	}

	public void ShowMenu() {
		while (true) {
			Console.WriteLine("\n=== Simple editor ===\n" +
					"1 — Open the file\n" +
					"2 — Edit the file\n" +
					"3 — Undo the last change\n" +
					"4 — Keyword search\n" +
					"5 — Load from binary\n" +
					"6 — Load from XML\n" +
					"7 — Save the file\n" +
					"8 — Save as binary\n" +
					"9 — Save as XML\n" +
					"10 — Exit\n" +
					"Your choice: ");

			string choice = Console.ReadLine();

			switch (choice) {
				case "1":
					Console.Write("The file path: ");
					OpenFile(Console.ReadLine());
					break;
				case "2":
					EditFile();
					break;
				case "3":
					Undo();
					break;
				case "4":
					SearchByKeywords();
					break;
				case "5":
					Console.Write("Enter path to binary file: ");
					DeserializeBinary(Console.ReadLine());
					break;
				case "6":
					Console.Write("Enter path to XML file: ");
					DeserializeXml(Console.ReadLine());
					break;
				case "7":
					SaveFile();
					break;
				case "8":
					Console.Write("Enter path to save binary file: ");
					SerializeBinary(Console.ReadLine());
					break;
				case "9":
					Console.Write("Enter path to save XML file: ");
					SerializeXml(Console.ReadLine());
					break;
				case "10":
					Console.WriteLine("The program is ending...");
					Environment.Exit(0);
					break;
				default:
					Console.WriteLine("Wrong choice, try again.");
					break;
			}
		}
	}
}
