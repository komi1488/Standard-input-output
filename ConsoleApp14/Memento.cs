using System;
using System.Collections.Generic;
using System.IO;

public class TextFileMemento {
  public string SavedContent { get; private set; }

  public TextFileMemento(string content) {
    SavedContent = content;
  }
}

public class UndoableTextFile : SimpleTextFile {
  private Stack<TextFileMemento> _history = new Stack<TextFileMemento>();
  public UndoableTextFile(string path, string content = "") : base(path, content) { }

  private void SaveState() {
    _history.Push(new TextFileMemento(Content));
  }

  public void ChangeContent(string newContent) {
    SaveState();  
    Content = newContent;  
  }

  public bool UndoLastChange() {
    if (_history.Count == 0)
      return false;  

    TextFileMemento previousState = _history.Pop();
    Content = previousState.SavedContent;
    return true;
  }
}
