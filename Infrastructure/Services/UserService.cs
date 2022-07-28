using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.RepositoryContracts;
using ApplicationCore.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        public async Task<bool> AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            if (await FavoriteExists(favoriteRequest.UserId, favoriteRequest.MovieId) == false)
            {
                var favorite = new Favorite
                {
                    MovieId = favoriteRequest.MovieId,
                    UserId = favoriteRequest.UserId
                };
                var savedFav = await _favoriteRepository.Add(favorite);
                return true;
            }
            return false;
            
        }

        public async Task AddMovieReview(ReviewRequestModel reviewRequest)
        {
            var review = new Review
            {
                MovieId = reviewRequest.MovieId,
                UserId = reviewRequest.UserId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText
            };
            var savedReview = await _reviewRepository.Add(review);
        }

        public async Task<bool> DeleteMovieReview(int userId, int movieId)
        {
            var reviews = await GetAllReviewsByUser(userId);
            var deleteReview = reviews.FirstOrDefault(r => r.MovieId == movieId);
            if (deleteReview != null)
            {
                await _reviewRepository.Delete(deleteReview);
                return true;
            }
            return false;
        }

        public async Task<bool> FavoriteExists(int id, int movieId)
        {
            var favorites = await GetAllFavoritesForUser(id);
            foreach (var fav in favorites)
            {
                if (fav.Id== movieId)
                {
                    return true;
                }
            }
            return false;

        }

        public async Task<List<MovieCardModel>> GetAllFavoritesForUser(int id)
        {
            var user = await _userRepository.GetById(id);
            var favorites = user.Favorites;
            var movieCards = new List<MovieCardModel>();
            foreach(var favorite in favorites)
            {
                movieCards.Add(new MovieCardModel
                {
                    Id = favorite.MovieId,
                    PosterUrl = favorite.Movie.PosterUrl,
                    Title = favorite.Movie.Title
                });
            }
            return movieCards;
        }

        public async Task<List<MovieCardModel>> GetAllPurchasesForUser(int id)
        {
            var user = await _userRepository.GetById(id);
            var purchases = user.Purchases;
            var movieCards = new List<MovieCardModel>();
            foreach (var purchase in purchases)
            {
                movieCards.Add(new MovieCardModel
                {
                    Id = purchase.MovieId,
                    PosterUrl = purchase.Movie.PosterUrl,
                    Title = purchase.Movie.Title
                });
            }
            return movieCards;
        }

        public async Task<ICollection<Review>> GetAllReviewsByUser(int id)
        {
            var user = await _userRepository.GetById(id);
            return user.Reviews;
        }

        public async Task<Purchase> GetPurchasesDetails(int userId, int movieId)
        {
            var user = await _userRepository.GetById(userId);
            var purchases = user.Purchases;
            var purchaseDetails = purchases.FirstOrDefault(p => p.MovieId == movieId);
            return purchaseDetails;
        }

        public Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            throw new NotImplementedException();
        }
    }
}
