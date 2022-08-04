using ApplicationCore.Entities;
using ApplicationCore.RepositoryContracts;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Models;

namespace Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        //injecting DbContext
        private readonly MovieShopDbContext _movieShopDbContext;
        public MovieRepository(MovieShopDbContext movieShopDbContext)
        {
            _movieShopDbContext = movieShopDbContext;
        }
        public async Task<Movie> GetById(int id)
        {
            var movieDetails = await _movieShopDbContext.Movies
                .Include(m => m.GenresOfMovie).ThenInclude(m => m.Genre)
                .Include(m => m.CastsOfMovie).ThenInclude(m => m.Cast)
                .Include(m => m.Trailers)
                .Include(m=>m.ReviewsOfMovie)
                .FirstOrDefaultAsync(m => m.Id == id);
            return movieDetails;
        }

		public async Task<PagedResultSet<Movie>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int page = 1)
		{
            // get total row count
            var totalMovieCountOfGenre = await _movieShopDbContext.MovieGenres.Where(g => g.GenreId == genreId).CountAsync();
			if (totalMovieCountOfGenre == 0)
			{
                throw new Exception("There is no movie with this genre");
			}

            // get actual data
            var movies = await _movieShopDbContext.MovieGenres.Where(g => g.GenreId == genreId).Include(g => g.Movie)
                .OrderByDescending(m => m.Movie.Revenue)
                .Select(m => new Movie // select moves from one object to another object
                {
                    Id = m.MovieId,
                    PosterUrl = m.Movie.PosterUrl,
                    Title = m.Movie.Title
                })
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSet<Movie>(movies, page, pageSize, totalMovieCountOfGenre);
            return pagedMovies;
		}

		public async Task<List<Movie>> GetTop30HighestRevenueMovies()
        {
            // we call the database with EF Core and get the relevant data
            // Use MovieShopDbContext and Movies DbSet

            // select Top30 from Movies Order by Revenue
            // we need to write corresponding LINQ query
            var movies = await _movieShopDbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();
            return movies;
        }

        public async Task<List<Movie>> GetTop30RatedMovies()
        {
            var movies = await _movieShopDbContext.Movies.OrderByDescending(m => m.Rating).Take(30).ToListAsync();
            return movies;
        }
    }
}