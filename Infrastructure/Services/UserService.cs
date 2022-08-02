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
		public UserService(IUserRepository userRepository, IReviewRepository reviewRepository, IPurchaseRepository purchaseRepository, IFavoriteRepository favoriteRepository)
		{
			_userRepository = userRepository;
			_reviewRepository = reviewRepository;
			_purchaseRepository = purchaseRepository;
			_favoriteRepository = favoriteRepository;
		}

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
            var reviewCheck = await _reviewRepository.GetById(reviewRequest.UserId, reviewRequest.MovieId);
            if (reviewCheck != null)
			{
                throw new Exception("Review already exists.");
			}
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
            var review = await _reviewRepository.GetById(userId, movieId);
            if (review != null)
            {
                await _reviewRepository.Delete(userId, movieId);
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

        public async Task<List<MovieCardModel>> GetAllFavoritesForUser(int userId)
        {
            var favorites = await _favoriteRepository.GetAll(userId);

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

        public async Task<List<MovieCardModel>> GetAllPurchasesForUser(int userId)
        {
            var purchases = await _purchaseRepository.GetAll(userId);//2
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

        public async Task<PurchaseDetailsModel> GetPurchasesDetails(int userId, int movieId)
        {
            var user = await _userRepository.GetById(userId);
            var purchases = user.Purchases;
            var purchaseDetails = purchases.FirstOrDefault(p => p.MovieId == movieId);
            if (purchaseDetails == null)
			{
                throw new NullReferenceException("There is no available purchase detail.");
			}
            PurchaseDetailsModel purchase = new()
            {
                UserId = userId,
                PurchaseNumber = purchaseDetails.PurchaseNumber,
                MovieId = movieId,
                PurchaseDateTime = purchaseDetails.PurchaseDateTime,
                TotalPrice = purchaseDetails.TotalPrice,
                PosterUrl = purchaseDetails.Movie.PosterUrl
             };
            return purchase;
        }
        public async Task<Review> GetReviewDetails(int userId, int movieId)
        {
            var review = await _reviewRepository.GetById(userId,movieId);
            if (review == null)
            {
                return null;
            }
            return review;
        }

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            var purchases = await _purchaseRepository.GetUserMovie(userId, purchaseRequest.MovieId);
            if (purchases == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId)
        {
            if (await IsMoviePurchased(purchaseRequest, userId) == false)
            {
                var purchase = new Purchase
                {
                    MovieId = purchaseRequest.MovieId,
                    UserId = userId,
                    PurchaseNumber = Guid.NewGuid(),
                    TotalPrice = purchaseRequest.TotalPrice,
                    PurchaseDateTime = purchaseRequest.PurchaseDateTime
                };
                var purchased = await _purchaseRepository.Add(purchase);
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            var remove = await _favoriteRepository.GetFavoriteById(favoriteRequest.UserId, favoriteRequest.MovieId);
            if (remove != null)
            {
                await _favoriteRepository.Delete(remove);
                return true;
            }
            return false;
        }

        public async Task UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            await DeleteMovieReview(reviewRequest.UserId, reviewRequest.MovieId);
            await AddMovieReview(reviewRequest);
        }
    }
}
