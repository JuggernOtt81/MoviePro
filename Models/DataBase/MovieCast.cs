namespace MoviePro.Models.DataBase
{
    public class MovieCast
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int CastID { get; set; }
        public string Department { get; set; }
        public string Name { get; set; }
        public string Character { get; set; }
        public string ImageUrl { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
