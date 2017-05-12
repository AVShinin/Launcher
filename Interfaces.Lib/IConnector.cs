using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//[ComImport()]
//[Guid("CB36B2F6-914B-44DA-8464-0032BA59676D")]
public interface IConnector
{
    void Initial(IConfig config, IStory story);
}
