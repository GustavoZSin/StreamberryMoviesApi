namespace StreamberryMoviesApi.Data.Dtos
{
    public class ReadStreamingPlatformDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ReadMovieStreamingAssociationDto> ServicesOfStreaming { get; set; }
    }
}
