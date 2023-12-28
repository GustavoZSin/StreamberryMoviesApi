namespace StreamberryMoviesApi.Models
{
    public class MovieStreamingAssociation
    {
        public int? MovieId { get; set; }
        public virtual Movie Movie { get; set; }
        public int? StreamingServiceId { get; set; }
        public virtual StreamingPlatform StreamingPlatform { get; set; }
    }
}