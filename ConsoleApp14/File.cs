using System;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SimpleTextFile {
  public string Path { get; set; }
  public string Content { get; set; }

  public SimpleTextFile(string path, string content = "") {
    Path = path;
    Content = content;
  }

  public void Save() {
    File.WriteAllText(Path, Content);
  }

  public void Load() {
    Content = File.ReadAllText(Path);
  }

  public void SerializeBinary(string filePath) {
    using (FileStream stream = new FileStream(filePath, FileMode.Create)) {
      BinaryFormatter formatter = new BinaryFormatter();
      formatter.Serialize(stream, this);
    }
  }

  public void SerializeXml(string filePath) {
    XmlSerializer serializer = new XmlSerializer(typeof(SimpleTextFile));
    using (TextWriter writer = new StreamWriter(filePath)) {
      serializer.Serialize(writer, this);
    }
  }
}
