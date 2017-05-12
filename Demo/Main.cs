using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

public class Main : IConnector
{
    public void Initial(IConfig config, IStory story)
    {
        var args = story.Get("launcher").Get<string[]>("args");
        story.Get("launcher").AddOrUpdate("mainwindow", new Demo.MainWindowDemo(args));
    }
}
