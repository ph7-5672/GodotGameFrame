using System.Collections.Generic;
using Frame.Common;
using Godot;

namespace Frame.Module
{
    public class ModuleDatatable : Singleton<ModuleDatatable>
    {

        private readonly List<IReadOnlyList<string>>[] tables = new List<IReadOnlyList<string>>[Constants.datatableTypeArray.Length];

        public void Load(DatatableType datatableType)
        {
            var file = new File();
            file.Open($"{Constants.resourceRoot}Database/{datatableType}{Constants.databaseSuffix}", File.ModeFlags.Read);
            var index = (int) datatableType;
            var list = tables[index];
            if (list == null)
            {
                list = new List<IReadOnlyList<string>>();
            }

            while (!file.EofReached())
            {
                var line = file.GetCsvLine("\t");
                //result.Add(line);
                if (line[0].Equals("*"))
                {
                }
                else if (int.TryParse(line[0], out var id))
                {
                    list.Add(line);
                }
            }
            
            tables[index] = list;
            file.Close();
            file.Dispose();
        }



        public IReadOnlyList<string> GetDatatable(DatatableType datatableType, int index)
        {
            var table = tables[(int) datatableType];
            return table[index];
        }


    }
    
    
    
}