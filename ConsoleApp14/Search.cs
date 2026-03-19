using System;
using System.Collections.Generic;
using System.IO;

public class FileSearcher {
  public List<string> FindFilesWithKeywords(string directory, List<string> keywords) {
    List<string> foundFiles = new List<string>();
    string[] allFiles = Directory.GetFiles(directory, "*.txt");
    
    int fileNum;
    int keywordIndex;

    for (fileNum = 0; fileNum < allFiles.Length; ++fileNum) {
      string file = allFiles[fileNum];
      string content = File.ReadAllText(file);
      bool allKeywordsFound = true;

      for (keywordIndex = 0; keywordIndex < keywords.Count; ++keywordIndex) {
        string keyword = keywords[keywordIndex];
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
