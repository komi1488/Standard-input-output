using System;
using System.Collections.Generic;
using System.Linq;

public class SimpleEditor {
  private UndoableTextFile _currentFile;

  public void OpenFile(string filePath) {
    _currentFile = new UndoableTextFile(filePath);
    _currentFile.Load();
    Console.WriteLine("Файл открыт. Текущее содержимое:");
    Console.WriteLine(_currentFile.Content);
  }

  public void EditFile() {
    Console.WriteLine("Введите новый текст (в конце напишите 'SAVE'):");
    List<string> lines = new List<string>();

    string line;
    while ((line = Console.ReadLine()) != "SAVE") {
      lines.Add(line);
    }

    string newContent = string.Join("\n", lines);
    _currentFile.ChangeContent(newContent);
    Console.WriteLine("Текст изменён!");
  }

  public void Undo() {
    if (_currentFile.UndoLastChange()) {
      Console.WriteLine("Откат выполнен. Новое содержимое:");
      Console.WriteLine(_currentFile.Content);
    } else {
      Console.WriteLine("Нет изменений для отката.");
    }
  }
  
  public void SearchByKeywords() {
    SimpleIndexer indexer = new SimpleIndexer();

    Console.Write("Введите путь к папке для поиска: ");
    string directory = Console.ReadLine();

    Console.Write("Введите ключевые слова для поиска (через пробел): ");
    string input = Console.ReadLine();
    List<string> keywords = input.Split(' ').ToList();

    try {
        Dictionary<string, List<string>> index = indexer.CreateIndex(directory, keywords);

        indexer.DisplayIndex(index);
    } catch (Exception ex) {
        Console.WriteLine($"Произошла ошибка при поиске: {ex.Message}");
    }
  }

  public void SaveFile() {
    _currentFile.Save();
    Console.WriteLine("Файл сохранён!");
  }

  public void ShowMenu() {
    while (true) {
      Console.WriteLine("\n=== Простой редактор ===");
      Console.WriteLine("1 — Открыть файл");
      Console.WriteLine("2 — Редактировать файл");
      Console.WriteLine("3 — Отменить последнее изменение");
      Console.WriteLine("4 — Поиск по ключевым словам");
      Console.WriteLine("5 — Сохранить файл");
      Console.WriteLine("6 — Выход");
      Console.Write("Ваш выбор: ");

      string choice = Console.ReadLine();

      switch (choice) {
        case "1":
          Console.Write("Путь к файлу: ");
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
          SaveFile();
          break;
        case "6":
          Console.WriteLine("Программа завершается...");
          Environment.Exit(0);
          break;
        default:
          Console.WriteLine("Неверный выбор, попробуйте снова.");
          break;
      }
    }
  }
}
