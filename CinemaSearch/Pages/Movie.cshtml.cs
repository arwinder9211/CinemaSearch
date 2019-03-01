using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;

namespace CinemaSearch.Pages
{
    public class MovieModel : PageModel
    {
        public string JSONresponseFromAPI { get; set; }
        public string Message { get; set; }
        public string MovieTitle;
        public string MovieRating;
        public string MoviePosterImage;
        public string MovieLanguage;
        public string MovieBio;
        public string MovieReleaseDate;

        public void OnGet()
        {

            string userEntry = Request.QueryString.Value;
            userEntry = userEntry.Split("=")[1];

            if (userEntry == null)
            {
                Response.Redirect("Index");
            }
            else
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.themoviedb.org/3/search/movie?api_key=a3bdaae66f8cf705750820e17c0e9471&query=" + userEntry);
                try
                {
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                        JSONresponseFromAPI = reader.ReadToEnd();

                        dynamic parsing = JObject.Parse(JSONresponseFromAPI);
                        try
                        {

                            MovieTitle = parsing.results[0].original_title;
                            MovieRating = parsing.results[0].vote_average;
                            MoviePosterImage = "https://image.tmdb.org/t/p/w300/" + parsing.results[0].poster_path;
                            MovieLanguage = parsing.results[0].original_language;
                            MovieBio = parsing.results[0].overview;
                            MovieReleaseDate = parsing.results[0].release_date;
                        }
                        catch (Exception e)
                        {
                            Message = "Looks like there's a spelling error!";
                        }
                    }
                }
                catch (WebException e)
                {
                    Message = "Oops! Try again!";
                }
            }
        }
    }
}
