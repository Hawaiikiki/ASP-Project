using ApplicationCore.ServiceContracts;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repositories;

namespace Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        // communicate with Repos
        public List<MovieCardModel> GetTopRevenueMovies()
        {
            var movieRepository = new MovieRepository();
            var movies = movieRepository.GetTop30HighestRevenueMovies();
            var movieCards = new List<MovieCardModel>();
            foreach (var movie in movies)
            {
                movieCards.Add(new MovieCardModel { Id = movie.Id, Title = movie.Title, PosterUrl = movie.PosterUrl });
            }
            return movieCards;
        }
    }
}
