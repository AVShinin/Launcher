using System.Runtime.InteropServices;

//[ComImport()]
//[Guid("948EE628-FB1E-41A6-890B-5AAF375E5A56")]
public interface IStoryItem
{
    string Group { get; }
    T Get<T>(string param);
    bool Add(string param, object value);
    void AddOrUpdate(string param, object value);
    bool Remove(string param);
}
