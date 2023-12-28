namespace StreamberryMoviesApi.Data.Dtos
{
    public class CreateMovieStreamingAssociationDto
    {
        public int MovieId { get; set; }
        public int StreamingServiceId { get; set; }
    }
}
