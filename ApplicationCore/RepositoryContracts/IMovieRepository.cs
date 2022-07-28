﻿using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.RepositoryContracts
{
    public interface IMovieRepository
    {
        // include CRUD Operations regarding Movie Table and use Movie Entity

        //async method should return task
        Task<List<Movie>> GetTop30HighestRevenueMovies();

        Task<List<Movie>> GetTop30RatedMovies();
        Task<Movie> GetById(int id);
        Task<PagedResultSet<Movie>> GetMoviesByGenrePagination(int genreId, int pageSize = 30, int page = 1);

    }
}
