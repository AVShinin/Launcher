using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

//[ComImport()]
//[Guid("28FEB2B5-7395-4621-8CA6-A6EA244F53B6")]
public interface IConfig
{
    IConfigGroup Get(string groupName);
    IConfigGroup Add(string groupName);
}