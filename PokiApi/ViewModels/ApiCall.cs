using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using PokiApi.Models;
using static PokiApi.Models.Pokemon;

namespace PokiApi.ViewModels
{
    internal class ApiCall : IDisposable
    {
        private readonly string _URL;
        private int _LimitFrom;
        private int _LimitTo;
        private HttpClient _HTTPClient;

        public string URL { get => _URL; }
        public int LimitFrom { get => _LimitFrom; }
        public int LimitTo { get => _LimitTo; }

        //Contructor for 1 parom to call one Pokemon
        public ApiCall(string Url)
        {
            _URL = Url;
            _HTTPClient = new HttpClient();
        }

        public ApiCall(string Url, int limitFrom, int limitTo)
        {
            _URL = Url;
            _LimitFrom = limitFrom;
            _LimitTo = limitTo;
            _HTTPClient = new HttpClient();
        }



        public async Task<Pokemon> GetResponse()
        {
            Pokemon pokemon = null;
            //Result result = null;
            HttpResponseMessage response = await _HTTPClient.GetAsync($"{_URL}?offset={_LimitFrom}&limit={_LimitTo}");

            if (response.IsSuccessStatusCode)
            {
                pokemon = await response.Content.ReadAsAsync<Pokemon>();
            }
            else
            {
                throw new Exception("Something is wrong!");
            }


            return pokemon;
        }

        public void Dispose()
        {
            _HTTPClient.Dispose();
        }

        public async Task<Root> CallPokemon()
        {
            Root pokemon = null;

            HttpResponseMessage mes = await _HTTPClient.GetAsync($"{_URL}");

            if (mes.IsSuccessStatusCode)
            {
                pokemon = await mes.Content.ReadAsAsync<Root>();
            }
            else
            {
                throw new Exception("Something is wrong!");
            }

            return pokemon;
            
        }

        public async Task<BitmapImage> CallPokemonGif(string url)
        { 
            BitmapImage img = null;

            HttpResponseMessage mes = await _HTTPClient.GetAsync($"{url}");

            if (mes.IsSuccessStatusCode)
            {
                img = await mes.Content.ReadAsAsync<BitmapImage>();
            }
            else
            {
                throw new Exception("Something is wrong!");
            }

            return img;
        }

    }
}
