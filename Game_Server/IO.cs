// Decompiled with JetBrains decompiler
// Type: Game_Server.IO
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using System;
using System.Linq;
using System.Xml;

namespace Game_Server
{
  public static class IO
  {
    public static string path = (string) null;
    public static string workingDirectory;

    public static string ReadValue(string section, string value)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(IO.path);
        return xmlDocument.DocumentElement.SelectNodes(section + "/" + value).Cast<XmlElement>().First<XmlElement>().InnerText;
      }
      catch
      {
        Log.WriteError("Error while reading " + section + " [" + value + "]");
      }
      return "0";
    }

    public static string ReadAttribute(string section, string value, string subvalue)
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(IO.path);
        XmlAttributeCollection attributes = xmlDocument.DocumentElement.SelectNodes(section + "/" + value).Cast<XmlElement>().First<XmlElement>().Attributes;
        if (attributes.Cast<XmlAttribute>().Where<XmlAttribute>((Func<XmlAttribute, bool>) (r => string.Compare(r.Name, subvalue, true) == 0)).Count<XmlAttribute>() > 0)
          return attributes.Cast<XmlAttribute>().Where<XmlAttribute>((Func<XmlAttribute, bool>) (r => string.Compare(r.Name, subvalue, true) == 0)).FirstOrDefault<XmlAttribute>().Value;
      }
      catch
      {
        Log.WriteError("Error while reading " + section + " [" + value + " _ " + subvalue + "]");
      }
      return (string) null;
    }
  }
}
