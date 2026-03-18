using System;
using System.Collections.Generic;
using System.IO;

public class TextFileState {
  public string SavedContent { get; private set; }

  public TextFileState(string content) {
    SavedContent = content;
  }
}

public class UndoableTextFile : SimpleTextFile {
  private Stack<TextFileState> _history = new Stack<TextFileState>();
  public UndoableTextFile(string path, string content = "") : base(path, content) { }

  private void SaveState() {
    _history.Push(new TextFileState(Content));
  }

  public void ChangeContent(string newContent) {
    SaveState();  
    Content = newContent;  
  }

  public bool UndoLastChange() {
    if (_history.Count == 0)
      return false;  

    TextFileState previousState = _history.Pop();
    Content = previousState.SavedContent;
    return true;
  }
}
