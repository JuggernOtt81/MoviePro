namespace MoviePro.Models.DataBase
{
    public class MovieCollection
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public int MovieId { get; set; }

        public int Order { get; set; }

        public virtual Collection Collection { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
