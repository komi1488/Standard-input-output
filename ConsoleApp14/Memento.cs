using System;
using System.Collections.Generic;


[Serializable]
public class TextFileMemento {
	public string Content { get; private set; }
	public DateTime Timestamp { get; private set; }
	public string SavedContent { get; set; }

	public TextFileMemento(string content) {
		Content = content;
		Timestamp = DateTime.Now;
	}
}

public class UndoableTextFile : SimpleTextFile, IOriginator {
	private Stack<TextFileMemento> _history = new Stack<TextFileMemento>();

	public UndoableTextFile(string path, string content = "") : base(path, content) { }

	object IOriginator.GetMemento() {
		var memento = new TextFileMemento(Content);
		_history.Push(memento);
		return memento;
	}

	void IOriginator.SetMemento(object memento) {
		if (memento is TextFileMemento textMemento) {
			Content = textMemento.Content;
			if (_history.Count > 0) _history.Pop();
		} else {
			throw new ArgumentException("Invalid memento type. Expected TextFileMemento.");
		}
	}

	public void ClearHistory() {
		_history.Clear();
		Console.WriteLine("History cleared.");
	}

	public void ChangeContent(string newContent) {
		Content = newContent;
	}
}


