using System;
using System.Collections.Generic;
using System.IO;

public class FileSearcher {
  public List<string> FindFilesWithKeywords(string directory, List<string> keywords) {
    List<string> foundFiles = new List<string>();
    string[] allFiles = Directory.GetFiles(directory, "*.txt");

    foreach (string file in allFiles) {
      string content = File.ReadAllText(file);
      bool allKeywordsFound = true;

      foreach (string keyword in keywords) {
        if (!content.Contains(keyword)) {
          allKeywordsFound = false;
          break;
        }
      }

      if (allKeywordsFound) {
        foundFiles.Add(file);
      }
    }

    return foundFiles;
  }
}
