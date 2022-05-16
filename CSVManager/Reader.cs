using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVManager
{
    public class Reader
    {
        public string[] Read(string path)
        {
            string[] content = System.IO.File.ReadAllLines(path);
            return content;
        }

        public string[] ReadAlternative(string path)
        {
            List<string> content = new List<string>();
            StreamReader reader = new StreamReader(path);
            string line = reader.ReadLine();

            //reader.Read() leest een byte (interpretatie van int)
            while (line != null)
            {
                //extra logica
                //productnummer > 100000 
                content.Add(line);
                line = reader.ReadLine();
            }

            reader.Close(); //heel belangrijk

            //meerdere bestanden kunnen het bestand bekijken maar enkel 1 app kan het bestand dit bewerken
            //processor 1 locked het bestand en zo kan een volgend bestand hier niet in bewerken
            //  dispose → garbage collector
            //(interface)


            return content.ToArray();
        }

        public string[] ReadAlternativeUsing(string path)
        {
            List<string> content = new List<string>();

            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    content.Add(line);
                    line = reader.ReadLine();
                }

                return content.ToArray();
            }
        }
    }
}
