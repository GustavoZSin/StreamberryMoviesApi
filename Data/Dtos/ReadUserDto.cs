namespace StreamberryMoviesApi.Data.Dtos
{
    public class ReadUserDto
    {
        public string UserName { get; set; }
        public string Id { get; set; }
        public DateTime ConsultHour { get; set; } = DateTime.Now;
    }
}
