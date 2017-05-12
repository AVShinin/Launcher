using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Manifest : IManifest
{
    public string Autor
    {
        get
        {
            return "Demo Autor";
        }
    }

    public DateTime DateCreate
    {
        get
        {
            return new DateTime(2016, 8, 16);
        }
    }

    public string Description
    {
        get
        {
            return "This library is demo project.";
        }
    }

    public Guid Guid
    {
        get
        {
            return new Guid("06F1B48E-B07F-4FAD-A45A-B768187E7967");
        }
    }

    public string Name
    {
        get
        {
            return "Demo";
        }
    }

    public string Title
    {
        get
        {
            return "Demo library";
        }
    }

    public Version Version
    {
        get
        {
            return new Version(1, 0);
        }
    }
}
