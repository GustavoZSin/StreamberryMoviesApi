namespace StreamberryMoviesApi.Data.Dtos
{
    public class UpdateMovieStreamingAssociationDto
    {
        public int MovieId { get; set; }
        public int StreamingServiceId { get; set; }
    }
}
