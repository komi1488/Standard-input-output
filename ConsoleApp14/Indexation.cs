using System;
using System.Collections.Generic;

public class SimpleIndexer {
  // Создаём индекс: какое слово в каких файлах встречается
  public Dictionary<string, List<string>> CreateIndex(string directory, List<string> keywords) {
    Dictionary<string, List<string>> index = new Dictionary<string, List<string>>();
    SimpleSearcher searcher = new SimpleSearcher();

    foreach (string word in keywords) {
      List<string> filesWithWord = searcher.FindFilesWithKeywords(directory, new List<string> { word });
      index[word] = filesWithWord;
    }

    return index;
  }

  public void DisplayIndex(Dictionary<string, List<string>> index) {
    Console.WriteLine("\n=== РЕЗУЛЬТАТЫ ИНДЕКСАЦИИ ===");

    foreach (var entry in index) {
      Console.WriteLine($"\nСлово '{entry.Key}' найдено в:");

      if (entry.Value.Count == 0) {
        Console.WriteLine("  — нигде не найдено");
      } else {
        foreach (string filePath in entry.Value) {
          Console.WriteLine($"  - {filePath}");
        }
      }
    }
  }
}
