using System;
using System.Collections.Generic;
using System.Security.Permissions;
using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class ModuleDatabase : Singleton<ModuleDatabase>
    {

        //private readonly Dictionary<DatabaseType, Dictionary<int, IData>> database = new Dictionary<DatabaseType, Dictionary<int, IData>>();

        private readonly Dictionary<DatabaseType, List<Dictionary<string, string>>> database =
            new Dictionary<DatabaseType, List<Dictionary<string, string>>>();

        public static Dictionary<DatabaseType, List<Dictionary<string, string>>> Database => Instance.database;
        
        
        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <returns></returns>
        public static void Load(DatabaseType databaseType)
        {
            if (!Instance.database.TryGetValue(databaseType, out var list))
            {
                list = LoadAsDictionary(databaseType);
                Instance.database.Add(databaseType, list);
            }
        }

        public static List<Dictionary<string, string>> LoadAsDictionary(DatabaseType databaseType)
        {
            var file = new File();
            file.Open($"{Constants.resourceRoot}Database/{databaseType}{Constants.databaseSuffix}", File.ModeFlags.Read);

            string[] titles = null;
            var result = new List<Dictionary<string, string>>();
            while (!file.EofReached())
            {
                var line = file.GetCsvLine();
                //result.Add(line);
                if (line[0].Equals("*"))
                {
                    titles = line;
                }
                else if (int.TryParse(line[0], out var id))
                {
                    var dictionary = new Dictionary<string, string>();
                    for (var i = 1; i < line.Length; ++i)
                    {
                        dictionary.Add(titles[i], line[i]);
                    }
                    result.Add(dictionary);
                }
            }

            file.Close();
            file.Dispose();

            return result;
        }


        public static Dictionary<string, string> GetData(DatabaseType databaseType, int index)
        {
            if (Instance.database.TryGetValue(databaseType, out var db))
            {
                return db[index];
            }

            return null;
        }


    }
}