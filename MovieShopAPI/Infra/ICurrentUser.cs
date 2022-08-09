namespace MovieShopAPI.Infra
{
    public interface ICurrentUser
    {
        public int UserId { get; }
        bool IsAdmin { get; }
        bool IsAuthenticated { get; }
        string Email { get; }
        string ProfilePictureUrl { get; }
        string FullName { get; }
    }
}
