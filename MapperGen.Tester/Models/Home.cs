using MapperGen.Tester.Attributes;

namespace MapperGen.Tester.Models
{
    [GenerateMappedDto]
    public class Home
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
