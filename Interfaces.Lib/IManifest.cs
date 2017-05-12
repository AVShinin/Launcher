using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

//[ComImport()]
//[Guid("CDBA5675-3A2F-40E6-B54B-8BC3CDB27C6B")]
public interface IManifest
{
    Guid Guid { get; }
    string Name { get; }
    Version Version { get; }
    DateTime DateCreate { get; }
    string Autor { get; }
    string Title { get; }
    string Description { get; }
}