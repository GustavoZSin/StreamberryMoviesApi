namespace StreamberryMoviesApi.Data.Dtos
{
    public class ReadRatingDto
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Commentary { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
    }
}
