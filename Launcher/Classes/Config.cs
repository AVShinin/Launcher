using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

public class Config : IConfig, IDisposable
{
    public static Config config = new Config();
    private List<IConfigGroup> __configGroups = new List<IConfigGroup>();

    public Config()
    {
        var _path_base = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        if (!File.Exists(System.IO.Path.Combine(_path_base.FullName, "setting.ini")))
            File.WriteAllText(System.IO.Path.Combine(_path_base.FullName, "setting.ini"), "CONFIG v1.0");

        Load();
    }

    public IConfigGroup Get(string groupName)
    {
        Load();
        if (__configGroups.Any(x => x.GroupName.Equals(groupName, StringComparison.OrdinalIgnoreCase)))
            return __configGroups.Find(x => x.GroupName.Equals(groupName, StringComparison.OrdinalIgnoreCase));
        return null;
    }

    public IConfigGroup Add(string groupName)
    {
        if (__configGroups.Any(x => x.GroupName.Equals(groupName, StringComparison.OrdinalIgnoreCase)))
            return __configGroups.Find(x => x.GroupName.Equals(groupName, StringComparison.OrdinalIgnoreCase));

        var group = new ConfigGroup(groupName, this);
        __configGroups.Add(group);
        Save();
        return group;
    }

    #region Save and Load methods
    public void Save()
    {
        StringBuilder str_bld = new StringBuilder();
        str_bld.AppendLine("CONFIG v1.0");

        foreach(var item in __configGroups)
        {
            str_bld.Append(item.ToString());
        }

        var _path_base = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        System.IO.File.WriteAllText(System.IO.Path.Combine(_path_base.FullName, "setting.ini"), str_bld.ToString());
    }
    
    public void Load()
    {
        //this.Add("PLUG", new Item<string[]>() { Name = "Paths", Value = new string[] { "./plug1", "../plugs", "../../../test","C:\\test" } });

        var _path_base = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        var lines = System.IO.File.ReadAllLines(System.IO.Path.Combine(_path_base.FullName, "setting.ini"));

        if (lines.Length<1 || !lines[0].StartsWith("CONFIG v1.0"))
            throw new Exception("Не верная начальная строка файла \"setting.ini\"");

        IConfigGroup tempGroup = null;
        foreach(var line in lines)
        {
            if (line.Length <= 0) continue;

            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                tempGroup = new ConfigGroup(line.Replace("[", "").Replace("]", ""), this);
                if(!__configGroups.Any(x=>x.GroupName.Equals(tempGroup.GroupName, StringComparison.OrdinalIgnoreCase)))
                    __configGroups.Add(tempGroup);
            }
            else if (tempGroup != null)
            {
                string name = "";
                foreach (var ch in line) if (ch != '=') name += ch; else break;
                if (string.IsNullOrWhiteSpace(name)) continue;

                string value = line.Substring(name.Length + 1);
                if (value.StartsWith("[") && value.EndsWith("]"))
                {
                    string[] items = value.Replace("[", "").Replace("]", "").Split(',');
                    tempGroup.Add(name, items.Select(x => x.Replace("#&9ee;", ",")).ToArray());
                }
                else tempGroup.Add(name, value);
            }
        }

    }

    public void Dispose()
    {
        Save();
    }
    #endregion
}

public class ConfigGroup : IConfigGroup
{
    public Config Parent { get; set; }
    public string GroupName { get; protected set; }
    private Dictionary<string, object[]> __items = new Dictionary<string, object[]>();

    public ConfigGroup(string groupName, Config parent)
    {
        this.GroupName = groupName;
        this.Parent = parent;
    }
    public bool Add(string name, params object[] value)
    {
        if (__items.ContainsKey(name.ToUpper())) return false;
        __items.Add(name.ToUpper(), value);
        Parent.Save();
        return true;
    }
    public void AddOrUpdate(string name, params object[] value)
    {
        if (!Add(name, value)) __items[name.ToUpper()] = value;
        Parent.Save();
    }
    public bool Remove(string name)
    {
        var result = __items.Remove(name.ToUpper());
        Parent.Save();
        return result;
    }

    public T[] Get<T>(string name)
    {
        if (__items.ContainsKey(name.ToUpper()))
            return __items[name.ToUpper()].Select(x=>To<T>(x)).ToArray();
        else return null;
    }


    public override string ToString()
    {
        StringBuilder _buider = new StringBuilder();
        _buider.AppendLine($"[{GroupName.ToUpper()}]");

        foreach(var item in __items)
        {
            if (item.Value.Length > 1)
            {
                string array = "[";
                foreach (var item_array in item.Value)
                {
                    array += item_array.ToString().Replace(",", "#&9ee;");
                    if (item.Value.ToList().IndexOf(item_array) != item.Value.Length - 1) array += ",";
                }
                array += "]";
                _buider.AppendLine($"{item.Key}={array}");
            }
            else _buider.AppendLine($"{item.Key}={item.Value[0]}");
        }

        _buider.AppendLine();

        return _buider.ToString();
    }

    public T To<T>(object param)
    {
        var out_type = new TypeDelegator(typeof(T));
        if (out_type.Equals(typeof(int))) return (T)(int.Parse(param.ToString()) as object);
        if (out_type.Equals(typeof(long))) return (T)(long.Parse(param.ToString()) as object);
        if (out_type.Equals(typeof(double))) return (T)(double.Parse(param.ToString()) as object);
        if (out_type.Equals(typeof(decimal))) return (T)(decimal.Parse(param.ToString()) as object);
        if (out_type.Equals(typeof(float))) return (T)(float.Parse(param.ToString()) as object);
        if (out_type.Equals(typeof(Version))) return (T)(Version.Parse(param.ToString()) as object);
        if (out_type.Equals(typeof(DateTime))) return (T)(DateTime.Parse(param.ToString()) as object);
        if (out_type.Equals(typeof(string))) return (T)(param as object);
        else return (T)param;
    }
}
