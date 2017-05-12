using System;
using System.Runtime.InteropServices;

//[ComImport()]
//[Guid("A4D2471B-87D2-4E79-8312-D9111BBE0BED")]
public interface IStory
{
    IStoryItem Get(string group);

    IStoryItem Add(string group);
}