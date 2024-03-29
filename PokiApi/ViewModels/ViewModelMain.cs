﻿using System;
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
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace PokiApi.ViewModels
{
    public class ViewModelMain : ViewModelBase
    {
        public ICommand _PokeballCommand;
        public ICommand _SearchPokeCommand;
        public Pokemon _PokemonList;
        public Root _selectedPoki;


        AutoCompleteStringCollection acs;


        string path;
        bool IsGameStart = false;
        BitmapImage _Image;


        //ViewModels
        ViewModelPokedex viewPokedex { get; set; }

        //Visibility
        Visibility _tab2 = Visibility.Hidden;






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
        public Pokemon PokemonList { get { return _PokemonList; } set { SetValue(ref _PokemonList, value); } }
        
        public string Path { get => path; set => path = value; }
        public Root SelectedPoki
        {
            get => _selectedPoki;
            set 
            { 
                SetValue(ref _selectedPoki, value);
            } 
        }
        public List<Root> ListAllPokemon{ get => _ListAllPokemon; set { SetValue(ref _ListAllPokemon, value); } }
        public BitmapImage SelectedPokemonGIF 
        { 
            get
            {
                if (_Image is null)
                {
                    _Image = new BitmapImage();
                    _Image.BeginInit();
                    _Image.UriSource = new Uri("/Views/bulbasaur-3.gif", UriKind.Relative);
                    _Image.EndInit();
                }

                return _Image;
            }
            set
            { 
                SetValue(ref _Image, value); 
            } 
        }

        //setting
        public AutoCompleteStringCollection Acs { get => acs; set { SetValue(ref acs, value); } }
        public Visibility Tab2 { get => _tab2; set { SetValue(ref _tab2, value); } }



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

            foreach (var name in _PokemonList.Results)
            {
                acs.Add(name.Name);
                using (ApiCall ac = new ApiCall(name.Url))
                {
                    _ListAllPokemon.Add(await ac.CallPokemon());
                }
            }


        }


        private async Task SearchPokeMethodeAsync(object pokemonName)
        {


            //string name = (string)pokemonName;
            TextBox test = (TextBox)pokemonName;
            if (acs.Count <151)
            {
                MessageBox.Show("Loading " + Math.Round((acs.Count / 151.0) * 100,0).ToString() + "%");
            }
            else
            {
                if (!IsGameStart)
                {
                    test.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    test.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    test.AutoCompleteCustomSource = acs;

                    IsGameStart = true;
                    _tab2 = Visibility.Visible;
                    OnPropertyChanged("Tab2");

                }

                if (CheckIfPokemonNameCorrect(test.Text))
                {
                    foreach (var poki in _PokemonList.Results)
                    {
                        if (test.Text.Equals(poki.Name))
                        {
                            using (ApiCall ac = new ApiCall(poki.Url))
                            {
                                _selectedPoki = await ac.CallPokemon();

                                // set GIF

                                string uriStart = "https://play.pokemonshowdown.com/sprites/xyani/";
                                string uriNamePokemon = poki.Name;
                                string uriEnd = ".gif";

                                _Image = new BitmapImage();
                                _Image.BeginInit();
                                _Image.UriSource = new Uri(uriStart + uriNamePokemon + uriEnd, UriKind.Absolute);
                                _Image.EndInit();

                                OnPropertyChanged("SelectedPokemonGIF");
                            }


                        }
                    }

                    OnPropertyChanged("SelectedPoki");
                    OnPropertyChanged("GetTypePokemon");
                    //OnPropertyChanged("Image");
                }
                else
                {
                    MessageBox.Show("Can't find your Pokemon");
                }
            }
            

        }

        private bool CheckIfPokemonNameCorrect(string input)
        {
            foreach (var poki in _PokemonList.Results)
            {
                if (input.Equals(poki.Name))
                {
                    return true;
                }
            }

            return false;
        }

        private async void PokeballMehode(object o)
        {

            // random yes no

            if (GotIt())
            {
                string msg = SavePokemonToDex(_selectedPoki);
                if (msg == null)
                {

                    MessageBox.Show("Bravo!");

                    //_ListAllPokemon = new List<Root>();

                    ReadToDataGrid();
                }
                else
                {
                    MessageBox.Show(msg);
                }
                
            }
            else
            {
                MessageBox.Show("Sorry next time!");
            }
            
        }

        //Save catch Pokemon in Dex
        public string SavePokemonToDex(Root pokie)
        {

            string msg = null;
            if (_selectedPoki is null)
            {
                msg = "Please find your pokemon!";
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(path: path, append: true))
                {
                    //sw.WriteLine($"ID");
                    sw.WriteLine($"{pokie.name}");
                }               
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
            _ListAllPokemon.Clear();

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
