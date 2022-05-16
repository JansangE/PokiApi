using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PokiApi.Models;
using System.Windows.Forms;
using static PokiApi.Models.Pokemon;
using MessageBox = System.Windows.Forms.MessageBox;
using System.IO;

using CSVManager;

namespace PokiApi.ViewModels
{
    public class ViewModelMain : ViewModelBase
    {
        public ICommand _PokeballCommand;
        public ICommand _SearchPokeCommand;
        public Pokemon _PokemonList;
        public Root _selectedPoki;

        AutoCompleteStringCollection acs;

        private string _GetTypePokemon;
        string path;

        //ViewModels
        ViewModelPokedex viewPokedex { get; set; }





        public ViewModelMain() : this(new Pokemon())
        {
            acs = new AutoCompleteStringCollection();


            Init();
        }

        public ViewModelMain(Pokemon pokemon)
        {
            _PokemonList = pokemon;
        }

        //Properties
        public Pokemon PokemonList
        {
            get { return _PokemonList; }
            set { SetValue(ref _PokemonList, value); } 
        }

        public string Path { get => path; set => path = value; }

        public Root SelectedPoki
        {
            get { return _selectedPoki; }
            set { SetValue(ref _selectedPoki, value); }
        }
        public string GetTypePokemon
        {
            get => _GetTypePokemon;
            set
            {
                SetValue(ref _GetTypePokemon, value);
            }
        }

        public List<Root> ListPokemonDex
        {
            get => _ListAllPokemon;
            set
            {
                SetValue(ref _ListAllPokemon, value);
            }
        }

        public ICommand PokeballCommand
        {
            get
            {
                if (_PokeballCommand == null)
                {
                    _PokeballCommand = new RelayCommand((param) => PokeballMehode(param));
                }
                return _PokeballCommand;
            }
            set
            {
                _PokeballCommand = value;
            }
        }

        public ICommand SearchPokeCommand
        {
            get
            {
                if (_SearchPokeCommand == null)
                {
                    _SearchPokeCommand = new RelayCommand((param) => SearchPokeMethodeAsync(param));
                }
                return _SearchPokeCommand;
            }
            set
            {
                _SearchPokeCommand = value;
            }
        }

        public AutoCompleteStringCollection Acs 
        { 
            get => acs;
            set
            {
                SetValue(ref acs, value);
            }

        }

        private async void Init()
        {
            // ViewModels
            viewPokedex = new ViewModelPokedex();
            reader = new Reader();

            path = Environment.CurrentDirectory;

            path += "/savePokemondex/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += "save.cvs";


            // Connect API
            using (ApiCall ac = new ApiCall(Url: "https://pokeapi.co/api/v2/pokemon",0,151))
            {
                _PokemonList = await ac.GetResponse();
                //_resultjt = await ac.GetResponse();
                //MessageBox.Show(_Pokemonje.Results[25].Name);

            }

            while (_ListAllPokemon.Count < 151)
            {
                foreach (var name in _PokemonList.Results)
                {
                    acs.Add(name.Name);
                    using (ApiCall ac = new ApiCall(name.Url))
                    {
                        _ListAllPokemon.Add(await ac.CallPokemon());

                    }

                }

                
            }


        }


        private async Task SearchPokeMethodeAsync(object pokemonName)
        {

            if (_ListAllPokemon.Count < 151)
            {
                MessageBox.Show("Wait" + _ListAllPokemon.Count.ToString());
            }
            else
            {
                //string name = (string)pokemonName;
                TextBox test = (TextBox)pokemonName;


                //test.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //test.AutoCompleteSource = AutoCompleteSource.CustomSource;
                //test.AutoCompleteCustomSource = acs;

                foreach (var poki in _ListAllPokemon)
                {
                    if (test.Text.Equals(poki.name))
                    {
                        _selectedPoki = poki;
                    }
                }

                if (_selectedPoki is null)
                {
                    MessageBox.Show("Can't find your Pokemon");
                }
                else
                {
                    //_GetTypePokemon = string.Empty;
                    ////foreach (var item in _selectedPoki.types)
                    ////{
                    ////    _GetTypePokemon += item.type + "\n";
                    ////}

                }

                OnPropertyChanged("SelectedPoki");
                OnPropertyChanged("GetTypePokemon");


                //MessageBox.Show(_selectedPoki.Url);
            }
        }

        private async void PokeballMehode(object o)
        {

            // random yes no

            if (GotIt())
            {
                string msg = SavePokemonToDex(_selectedPoki);

                MessageBox.Show(msg);

                //_ListAllPokemon = new List<Root>();

                ReadToDataGrid();

                MessageBox.Show(_ListAllPokemon.Count.ToString());
            }
            else
            {
                MessageBox.Show("Sorry next time!");
            }
            



            string ok = "OK";

            MessageBox.Show(ok);
        }

        //Save catch Pokemon in Dex
        public string SavePokemonToDex(Root pokie)
        {
            string msg = null;
            try
            {

                using (StreamWriter sw = new StreamWriter(path: path, append: true))
                {
                    //sw.WriteLine($"ID");
                    sw.WriteLine($"{pokie.name}");

                }

                msg = "Bravo you Got it!";
            }
            catch (Exception)
            {

                msg = "Something Wrong CatchPokemon";
            }

            return msg;
        }

        //Random get Pokemon
        public bool GotIt()
        {


            return true;
        }

        #region Pokedex
        //Pokedex

        public Root _Pokedex;
        Reader reader;
        List<Root> _ListAllPokemon = new List<Root>();
        List<Root> _ListPokemonDex = new List<Root>();
        List<Root> ListPokemonDex1
        {
            get => _ListPokemonDex;
            set
            {
                _ListPokemonDex = value;
                OnPropertyChanged();
            }
        } 
        public Root Pokedex 
        {
            get => _Pokedex;
            set
            {
                SetValue(ref _Pokedex, value);
            }
        }

        public void ReadToDataGrid()
        {
            string[] content = reader.ReadAlternativeUsing(path);
            //List<Root> list = new List<Root>();

            foreach (var p in _ListAllPokemon)
            {
                foreach (var item in content)
                {
                    if (p.name == item)
                    {
                        _ListPokemonDex.Add(p);
                    }
                    else
                    {
                        _ListPokemonDex.Add(new Root());
                    }

                }
            }

            //When add to list get value more than excpected don't know why
        }


        #endregion



    }
}
