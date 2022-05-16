using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokiApi.ViewModels
{
    public class ViewModelPokedex : ViewModelBase
    {
        string path;


        public ViewModelPokedex()
        {
            init();
        }

        private void init()
        {
            path = Environment.CurrentDirectory;

            path += "/savePokemondex/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


        }
    }
}
