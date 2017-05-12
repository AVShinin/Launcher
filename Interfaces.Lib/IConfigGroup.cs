using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IConfigGroup
{
    string GroupName { get; }
    bool Add(string name, params object[] value);
    void AddOrUpdate(string name, params object[] value);
    bool Remove(string name);

    T[] Get<T>(string name);
}
