using System;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SimpleTextFile {
  public string FilePath { get; set; }
  public string Content { get; set; }

  public SimpleTextFile(string path, string content = "") {
    FilePath = path;
    Content = content;
  }

  public void Save() {
    File.WriteAllText(FilePath, Content);
  }

  public void Load() {
    Content = File.ReadAllText(FilePath);
  }

  public void SerializeBinary(string filePath) {
    using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
      BinaryFormatter formatter = new BinaryFormatter();
      formatter.Serialize(stream, this);
    }
  }

  public void DeserializeBinary(string filePath) { 
    using (FileStream stream = new FileStream (filePath, FileMode.Open)) { 
      BinaryFormatter formatter = new BinaryFormatter();
      SimpleTextFile tempFile = (SimpleTextFile)formatter.Deserialize(stream);
      
      this.FilePath = tempFile.FilePath;
      this.Content = tempFile.Content;
    }
  }

  public void SerializeXml(string filePath) {
    XmlSerializer serializer = new XmlSerializer(typeof(SimpleTextFile));
    using (TextWriter writer = new StreamWriter(filePath)) {
      serializer.Serialize(writer, this);
    }
  }

  public void DeserializeXml(string filePath) {
    XmlSerializer serializer = new XmlSerializer(typeof(SimpleTextFile));
    using (TextReader reader = new StreamReader(filePath)) {
      SimpleTextFile tempFile = (SimpleTextFile)serializer.Deserialize(reader);
        
      this.FilePath = tempFile.FilePath;
      this.Content = tempFile.Content;
    }
  }
}
