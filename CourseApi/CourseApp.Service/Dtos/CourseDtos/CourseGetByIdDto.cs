namespace CourseApi.Dtos.CourseDtos
{
    public class CourseGetByIdDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Limit { get; set; }
        public int StudentCount { get; set; }
    }
}
