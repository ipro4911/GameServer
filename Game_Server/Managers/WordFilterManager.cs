// Decompiled with JetBrains decompiler
// Type: Game_Server.Managers.WordFilterManager
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Game_Server.Managers
{
  internal class WordFilterManager
  {
    public static List<WordFilter> filters = new List<WordFilter>();

    public static void Load()
    {
      WordFilterManager.filters.Clear();
      DataTable dataTable = DB.RunReader("SELECT * FROM wordfilter");
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        DataRow row = dataTable.Rows[index];
        if (row != null)
        {
          WordFilter wordFilter = new WordFilter(row["normal"].ToString(), row["replace"].ToString());
          WordFilterManager.filters.Add(wordFilter);
        }
      }
      Log.WriteLine("Successfully loaded [" + (object) WordFilterManager.filters.Count + "] Word Filters");
    }

    public static string RemoveSpecialCharacters(string str)
    {
      return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }

    public static string Replace(string text)
    {
      string[] strArray = text.Split(' ');
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = WordFilterManager.ReplaceWord(strArray[index]);
      return string.Join(" ", strArray);
    }

    private static string ReplaceWord(string p)
    {
      foreach (WordFilter filter in WordFilterManager.filters)
      {
        if (WordFilterManager.RemoveSpecialCharacters(p.ToLower()) == WordFilterManager.RemoveSpecialCharacters(filter.normal.ToLower()))
          return filter.replace;
      }
      return p;
    }
  }
}
