﻿using ApplicationCore.ServiceContracts;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using ApplicationCore.RepositoryContracts;

namespace Infrastructure.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<List<GenreModel>> GetAllGenres()
        {
            var genres = await _genreRepository.GetAllGenres();
            var genresModels = genres.Select(g => new GenreModel { Id = g.Id, Name = g.Name }).ToList();
            return genresModels;

        }
    }
}
