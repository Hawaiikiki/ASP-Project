﻿using ApplicationCore.Entities;
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

        List<Movie> GetTop30HighestRevenueMovies();

        List<Movie> GetTop30RatedMovies();
        Movie GetById(int id);

    }
}
