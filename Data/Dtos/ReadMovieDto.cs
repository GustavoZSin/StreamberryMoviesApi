namespace StreamberryMoviesApi.Data.Dtos
{
    public class ReadMovieDto
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public double MovieLenght { get; set; }
        public string MonthAndYear { get; set; }
        public DateTime ConsultHour { get; set; } = DateTime.Now;
        public ICollection<ReadMovieStreamingAssociationDto> ServicesOfStreaming { get; set; }

    }
}
