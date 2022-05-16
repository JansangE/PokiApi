using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokiApi.Models
{
    public class PokemonBase 
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public object Previous { get; set; }
        public List<ResultPokemon> Results { get; set; }
    }
}
