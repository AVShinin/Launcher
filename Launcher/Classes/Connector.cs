using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Connector
{
    public Dictionary<IManifest, IConnector> plugs { get; protected set; } = new Dictionary<IManifest, IConnector>();

    public void InitialPlugins()
    {
        var _path_base = new System.IO.DirectoryInfo(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        List<System.IO.DirectoryInfo> dirs = new List<System.IO.DirectoryInfo>();

        #region Paths parse
        
        if (Config.config.Get("plug") != null)
        {
            var _paths_string = Config.config.Get("plug").Get<string>("paths");
            if (_paths_string != null && _paths_string.Length > 0)
            {
                foreach (var _path_string in _paths_string)
                {
                    if (string.IsNullOrWhiteSpace(_path_string)) break;
                    System.IO.DirectoryInfo dir;
                    TryGetPath(_path_string, _path_base, out dir);
                    dirs.Add(dir);
                }
            }
        }

        if(dirs.Count<1)
        {
            dirs.Add(_path_base);
            dirs.Add(new System.IO.DirectoryInfo(System.IO.Path.Combine(_path_base.FullName, "plugs")));
        }
        #endregion

        #region Load Dll files
        foreach (var dir in dirs.Where(w => w.Exists))
        {
            foreach (var file in dir.GetFiles("*.dll", System.IO.SearchOption.TopDirectoryOnly))
            {
                var _assembly = System.Reflection.Assembly.LoadFile(file.FullName);

                IManifest manifest = null;
                IConnector connector = null;

                foreach (var _main in _assembly.GetTypes().Where(w => w.GetInterface(nameof(IConnector)) != null || w.GetInterface(nameof(IManifest)) != null).ToList())
                { 
                    var runnable = Activator.CreateInstance(_main);
                    if (runnable != null && runnable is IManifest) manifest = (IManifest)runnable;
                    if (runnable != null && runnable is IConnector) connector = (IConnector)runnable;
                }

                if (manifest != null && connector != null && !plugs.ContainsKey(manifest))
                {
                    plugs.Add(manifest, connector);
                    connector.Initial(Config.config, Story.story);
                }
            }
        }
        #endregion
    }
    private bool TryGetPath(string _path_string, System.IO.DirectoryInfo dir, out System.IO.DirectoryInfo path)
    {
        if (string.IsNullOrWhiteSpace(_path_string))
        {
            path = dir;
            return false;
        }
        if(_path_string.StartsWith("./") || _path_string.StartsWith("../"))
        {
            if (_path_string.StartsWith("../"))
            {
                string _path = _path_string.Substring(3);
                if (_path.StartsWith("../"))
                {
                    return TryGetPath(_path, dir.Parent, out path);
                }
                else
                {
                    path = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.Parent.FullName, _path));
                    return false;
                }
            }
            if (_path_string.StartsWith("./"))
            {
                string _path = _path_string.Substring(2);
                path = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, _path));
                return false;
            }
        }

        path = new System.IO.DirectoryInfo(_path_string);
        return false;
    }
}
