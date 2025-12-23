namespace SMS.API.DTOs
{
    public class CreateClassDto
    {
        public string Name { get; set; }
        public string Section { get; set; }
    }

    public class CreateSubjectDto
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }
    }
}
