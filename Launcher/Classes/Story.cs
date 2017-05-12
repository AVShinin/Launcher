using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Story : IStory
{
    public static Story story = new Story();
    private List<ItemStory> _items = new List<ItemStory>();

    public IStoryItem Get(string groupName)
    {
        if (_items.Any(x=>x.Group.Equals(groupName, StringComparison.OrdinalIgnoreCase)))
            return _items.Find(x => x.Group.Equals(groupName, StringComparison.OrdinalIgnoreCase));
        return null;
    }

    public IStoryItem Add(string groupName)
    {
        if (_items.Any(x => x.Group.Equals(groupName, StringComparison.OrdinalIgnoreCase)))
            return Get(groupName);
        var temp = new ItemStory(groupName);
        _items.Add(temp);
        return temp;
    }
}

public class ItemStory : IStoryItem
{
    private Dictionary<string, object> __items = new Dictionary<string, object>();
    public string Group { get; protected set; }

    public ItemStory(string group)
    {
        this.Group = group;
    }

    public T Get<T>(string param)
    {
        if (!__items.ContainsKey(param.ToUpper())) return default(T);
        return (T)__items[param.ToUpper()];
    }

    public bool Add(string param, object value)
    {
        if (__items.ContainsKey(param.ToUpper())) return false;
        __items.Add(param.ToUpper(), value);
        return true;
    }

    public void AddOrUpdate(string param, object value)
    {
        if (!Add(param, value))
            __items[param.ToUpper()] = value;
    }

    public bool Remove(string param)
    {
        return __items.Remove(param);
    }
}
