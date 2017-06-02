using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace MagicGladiators
{
    public enum dbMatching { NotConnected = 0, NoMatch = 1, Match = 2, DuplicatesError = 3, UnknownError = 4 }
    public enum dbCreate { NotConnected = 0, Success = 1, AlreadyExist = 2, InvalidInput = 3 ,UnknownError = 4}
    public enum dbTables { login = 0 }

    public class dbCon
    {
        private static dbCon _i;
        public static dbCon i
        {
            get
            {
                if (_i == null)
                {
                    _i = new dbCon();
                }
                return _i;
            }
        }
        private dbCon()
        {
            if (!File.Exists(@"data.db"))
            {
                SQLiteConnection.CreateFile(@"data.db");
            }
            connection = new SQLiteConnection("Data Source=data.db;Version=3;");
            connection.Open();
            string sql;

            sql = "create table if not exists login (id integer primary key, name text, password text)";
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
            connection.Close();
            isConnected = false;

            connection.StateChange += UpdateIfOpen;
        }

        //

        private SQLiteConnection connection;
        private bool isConnected;

        public void StartDataBaseConnection()
        {
            if (!isConnected)
            {
                connection.Open();
            }
        }

        public void CloseDataBaseConnection()
        {
            if (isConnected)
            {
                connection.Close();
            }
        }

        private void UpdateIfOpen(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Open)
            {
                isConnected = true;
            }
            if (e.CurrentState == ConnectionState.Closed || e.CurrentState == ConnectionState.Broken)
            {
                isConnected = false;
            }
        }

        public bool IsConnected { get { return isConnected; } }
        public bool IsDisconned { get { return !isConnected; } }

        public dbMatching IsMatch(dbTables table, string info1, string info2)
        {
            if (isConnected)
            {
                if (table == dbTables.login)
                {
                    string commandString = "select * from login where name='" + info1 + "' and password='" + info2 + "' ";
                    SQLiteCommand loginCommand = new SQLiteCommand(commandString, connection);
                    loginCommand.ExecuteNonQuery();
                    SQLiteDataReader dr = loginCommand.ExecuteReader();
                    int count = 0;
                    while (dr.Read())
                    {
                        count++;
                    }
                    dr.Close();
                    if (count == 1)
                    {
                        return dbMatching.Match;
                    } else if (count > 1)
                    {
                        return dbMatching.DuplicatesError;
                    } else if (count == 0)
                    {
                        commandString = "select * from login where name='" + info2 + "' and password='" + info1 + "' ";
                        loginCommand = new SQLiteCommand(commandString, connection);
                        loginCommand.ExecuteNonQuery();
                        dr = loginCommand.ExecuteReader();
                        count = 0;
                        while (dr.Read())
                        {
                            count++;
                        }
                        dr.Close();
                        if (count == 1)
                        {
                            return dbMatching.Match;
                        } else if (count > 1)
                        {
                            return dbMatching.DuplicatesError;
                        } else if (count == 0)
                        {
                            return dbMatching.NoMatch;
                        } else
                        {
                            return dbMatching.UnknownError;
                        }
                    } else
                    {
                        return dbMatching.UnknownError;
                    }
                }
                return dbMatching.NoMatch;
            } else
            {
                return dbMatching.NotConnected;
            }
        }

        public dbCreate CreateAccount(string name, string password)
        {
            if (isConnected)
            {
                if (Regex.IsMatch(name, @"^[A-Za-z0-9]+$") && Regex.IsMatch(password, @"^[A-Za-z0-9]+$"))
                {
                    string testforstring = "select * from login where name='" + name + "';";
                    SQLiteCommand testcommand = new SQLiteCommand(testforstring, connection);
                    testcommand.ExecuteNonQuery();
                    SQLiteDataReader testread = testcommand.ExecuteReader();
                    int count = 0;
                    while (testread.Read())
                    {
                        count++;
                    }
                    testread.Close();
                    if (count >= 1)
                    {
                        return dbCreate.AlreadyExist;

                    } else if (count == 0)
                    {
                        string tempCommand = "insert into login (id, name, password) values (null, '" + name + "', '" + password + "');";
                        SQLiteCommand command = new SQLiteCommand(tempCommand, connection);
                        command.ExecuteNonQuery();
                        return dbCreate.Success;
                    } else
                    {
                        return dbCreate.UnknownError;
                    }
                } else
                {
                    return dbCreate.InvalidInput;
                }
            } else
            {
                return dbCreate.NotConnected;
            }
        }
    }
}
