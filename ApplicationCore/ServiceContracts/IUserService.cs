﻿using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.ServiceContracts
{
    public interface IUserService
    {
        Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId);
        Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId);
        Task<List<MovieCardModel>> GetAllPurchasesForUser(int id);
        Task<Purchase> GetPurchasesDetails(int userId, int movieId);
        Task<bool> AddFavorite(FavoriteRequestModel favoriteRequest);
        Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest);
        Task<bool> FavoriteExists(int id, int movieId);
        Task<List<MovieCardModel>> GetAllFavoritesForUser(int id);
        Task AddMovieReview(ReviewRequestModel reviewRequest);
        Task<bool> UpdateMovieReview(ReviewRequestModel reviewRequest);
        Task<bool> DeleteMovieReview(int userId, int movieId);
        Task<ICollection<Review>> GetAllReviewsByUser(int id);
    }
}