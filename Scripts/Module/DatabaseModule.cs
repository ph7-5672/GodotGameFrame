using System;
using System.Collections.Generic;
using System.Linq;
using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class DatabaseModule : Singleton<DatabaseModule>
    {

        private readonly Dictionary<string, List<IData>> database = new Dictionary<string, List<IData>>();

        /// <summary>
        /// 加载数据表。
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] Load<T>() where T : IData, new()
        {
            var name = typeof(T).Name;
            if (!Instance.database.TryGetValue(name, out var list))
            {
                list = LoadFromFile<T>(name);
                Instance.database.Add(name, list);
            }
            return list.Cast<T>().ToArray();
        }


        private static List<IData> LoadFromFile<T>(string name) where T : IData, new()
        {
            var file = new File();
            file.Open($"{Constants.resourceRoot}Database/{name}{Constants.databaseSuffix}", File.ModeFlags.Read);

            string[] titles = null;
            var result = new List<IData>();
            
            while (!file.EofReached())
            {
                var line = file.GetCsvLine();
                if (line[0].Equals("*"))
                {
                    titles = line;
                }
                else if (int.TryParse(line[0], out var id))
                {
                    if (titles == null)
                    {
                        throw new Exception("表格格式错误：没有标题行。");
                    }

                    var data = new T
                    {
                        Id = id
                    };
                    
                    for (var i = 1; i < line.Length; i++)
                    {
                        var title = titles[i];
                        var value = line[i];
                        data.SetValue(title, value);
                    }
                    
                    result.Add(data);
                }
            }

            return result;
        }

    }
}