// Decompiled with JetBrains decompiler
// Type: Game_Server.DB
// Assembly: GameServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1C1430FE-9A2A-4A11-B0EE-D1D3878908AC
// Assembly location: C:\Users\Can\Desktop\WrMontana Public\GS\GameServer.exe

using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;

namespace Game_Server
{
  public static class DB
  {
    private static string strConnection;
    private static MySqlConnection dbConnection;

    private static void dbConnection_StateChange(object usr, StateChangeEventArgs ev)
    {
      if (ev.CurrentState == ConnectionState.Broken)
      {
        Thread.Sleep(1000);
        DB.dbConnection.Close();
      }
      if (ev.CurrentState != ConnectionState.Closed)
        return;
      Thread.Sleep(1000);
      Log.WriteLine("Reconnecting to SQL Server 1...");
      DB.dbConnection = new MySqlConnection(DB.strConnection);
      DB.dbConnection.StateChange += new StateChangeEventHandler(DB.dbConnection_StateChange);
      Thread.Sleep(2000);
      DB.dbConnection.Open();
      Log.WriteLine("Reconnection to database 1 successful.");
    }

    public static bool openConnection(
      string dbHost,
      int dbPort,
      string dbName,
      string dbUsername,
      string dbPassword,
      int dbPoolsize)
    {
      try
      {
        Log.WriteLine("Connecting to " + dbName + " at " + dbHost + ":" + (object) dbPort + " for user '" + dbUsername + "'");
        DB.strConnection = "Server=" + dbHost + ";Port=" + (object) dbPort + ";Database=" + dbName + ";User=" + dbUsername + ";Password=" + dbPassword + ";Connection Timeout=99999999;";
        DB.dbConnection = new MySqlConnection(DB.strConnection);
        DB.dbConnection.StateChange += new StateChangeEventHandler(DB.dbConnection_StateChange);
        DB.dbConnection.Open();
        if (DB.dbConnection.State == ConnectionState.Open)
        {
          Log.WriteLine("Connection to database successfull.");
          return true;
        }
        Log.WriteError("Failed to connect to " + dbName + " at " + dbHost + ":" + (object) dbPort + " for user '" + dbUsername + "'");
        return false;
      }
      catch (Exception ex)
      {
        Log.WriteError("Failed to connect! Error thrown was: " + ex.Message);
        return false;
      }
    }


        public static string[] runReadRow(string Query)
        {
            try
            {
                ArrayList builder = new ArrayList();
                using (MySqlConnection c = new MySqlConnection(strConnection))
                {
                    c.Open();
                    using (MySqlCommand cmd = c.CreateCommand())
                    {
                        cmd.CommandText = Query + " LIMIT 1";
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    try { builder.Add(reader[i].ToString()); }
                                    catch { builder.Add(""); }
                                }
                            }
                            reader.Close();
                            return (string[])builder.ToArray(typeof(string));
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Log.WriteError("Error '" + ex.Message + "' at '" + Query + "'");
                return new string[0];
            }
        }
        public static void closeConnection()
    {
      Log.WriteLine("Closing database connection...");
      try
      {
        DB.dbConnection.Close();
        Log.WriteLine("Database connection closed.");
      }
      catch
      {
        Log.WriteError("No database connection.");
      }
    }

    public static void RunQuery(string query)
    {
      DB.QueryAsync(new Statment(query));
    }

    private static void QueryAsync(Statment statement)
    {
      MySqlConnection mySqlConnection = new MySqlConnection(DB.strConnection);
      mySqlConnection.Open();
      MySqlCommand command = mySqlConnection.CreateCommand();
      command.CommandTimeout = 0;
      command.CommandText = statement.query;
      foreach (KeyValuePair<string, object> parameter in statement.parameters)
        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
      command.Prepare();
      command.BeginExecuteNonQuery(new AsyncCallback(DB.QueryAsyncCallback), (object) new DB.QueryObject<bool>((Action<bool>) null, command));
      statement.Dispose();
    }

    private static void QueryAsyncCallback(IAsyncResult iAr)
    {
      DB.QueryObject<bool> asyncState = (DB.QueryObject<bool>) iAr.AsyncState;
      try
      {
        MySqlConnection mySqlConnection = (MySqlConnection) null;
        using (MySqlCommand sqlCommand = asyncState.MySQLCommand)
        {
          sqlCommand.CommandTimeout = 0;
          sqlCommand.EndExecuteNonQuery(iAr);
          mySqlConnection = sqlCommand.Connection;
        }
        ((Component) mySqlConnection)?.Dispose();
        if (asyncState.OriginalCallback == null)
          return;
        asyncState.OriginalCallback(true);
      }
      catch (Exception ex)
      {
        if (asyncState.OriginalCallback != null)
          asyncState.OriginalCallback(false);
        Log.WriteError("Error while executing query: " + ex.Message);
      }
    }

    public static void RunQueryNotAsync(string query)
    {
      DB.Query(new Statment(query));
    }

    private static void Query(Statment statement)
    {
      using (MySqlConnection mySqlConnection = new MySqlConnection(DB.strConnection))
      {
        mySqlConnection.Open();
        using (MySqlCommand command = mySqlConnection.CreateCommand())
        {
          command.CommandTimeout = 0;
          command.CommandText = statement.query;
          foreach (KeyValuePair<string, object> parameter in statement.parameters)
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
          try
          {
            command.Prepare();
            command.ExecuteNonQuery();
          }
          catch (Exception ex)
          {
            Log.WriteError("Error while executing query: " + ex.Message);
          }
          statement.Dispose();
        }
      }
    }

    public static DataTable RunReader(string Query)
    {
      return DB.Read(new Statment(Query));
    }

    public static object RunReaderOnce(string var, string Query)
    {
      try
      {
        DataTable dataTable = DB.Read(new Statment(Query + " LIMIT 1"));
        if (dataTable.Rows.Count > 0)
          return (object) dataTable.Rows[0][var].ToString();
      }
      catch (Exception ex)
      {
        Log.WriteError("Error while executing query: " + ex.Message);
      }
      return new object();
    }

    private static DataTable Read(Statment statement)
    {
      DataTable dataTable = new DataTable();
      using (MySqlConnection mySqlConnection = new MySqlConnection(DB.strConnection))
      {
        mySqlConnection.Open();
        using (MySqlCommand command = mySqlConnection.CreateCommand())
        {
          command.CommandTimeout = 0;
          command.CommandText = statement.query;
          foreach (KeyValuePair<string, object> parameter in statement.parameters)
            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
          command.Prepare();
          try
          {
            dataTable.Load((IDataReader) command.ExecuteReader());
          }
          catch (Exception ex)
          {
            Log.WriteError("Error while executing query: " + ex.Message);
          }
          statement.Dispose();
        }
      }
      return dataTable;
    }

    public static bool checkExists(string Query)
    {
      try
      {
        using (MySqlConnection connection = new MySqlConnection(DB.strConnection))
        {
          connection.Open();
          using (MySqlCommand mySqlCommand = new MySqlCommand(Query + " LIMIT 1", connection))
            return mySqlCommand.ExecuteReader().HasRows;
        }
      }
      catch (Exception ex)
      {
        Log.WriteError("Error '" + ex.Message + "' at '" + Query + "'");
        return false;
      }
    }

    public static string Stripslash(string Query)
    {
      return Query.Replace("\\", "\\").Replace("'", "'").Replace("'", "\\'");
    }

    private struct QueryObject<T>
    {
      public Action<T> OriginalCallback;
      public MySqlCommand MySQLCommand;

      public QueryObject(Action<T> callback, MySqlCommand cmd)
      {
        this.OriginalCallback = callback;
        this.MySQLCommand = cmd;
        this.MySQLCommand.CommandTimeout = 0;
      }
    }
  }
}
