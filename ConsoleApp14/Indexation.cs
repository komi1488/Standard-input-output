using System;
using System.Collections.Generic;

public class FileIndexer {
  public Dictionary<string, List<string>> CreateIndex(string directory, List<string> keywords) {
    Dictionary<string, List<string>> index = new Dictionary<string, List<string>>();
    FileSearcher searcher = new FileSearcher();

    foreach (string word in keywords) {
      List<string> filesWithWord = searcher.FindFilesWithKeywords(directory, new List<string> { word });
      index[word] = filesWithWord;
    }

    return index;
  }

  public void DisplayIndex(Dictionary<string, List<string>> index) {
    Console.WriteLine("\n=== RESULTS OF INDEXATION ===");

    foreach (var entry in index) {
      Console.WriteLine($"\nWord '{entry.Key}' found in:");

      if (entry.Value.Count == 0) {
        Console.WriteLine("  — not found anywhere");
      } else {
        foreach (string filePath in entry.Value) {
          Console.WriteLine($"  - {filePath}");
        }
      }
    }
  }
}
