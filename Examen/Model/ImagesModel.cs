namespace Model
{
    public class ImagesModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required byte[] ImageData { get; set; }
    }
}