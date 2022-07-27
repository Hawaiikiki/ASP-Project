namespace MovieShopMVC.Infra
{
    public interface ICurrentUser
    {
        public int UserId { get;} // we don't need setter, since we shouldn't modify this object
        bool isAdmin { get;}
        bool isAuthenticated { get;}
        string Email { get; }
        string ProfilePictureUrl { get; }
        string FullName { get; }
    }
}