using MapperGen.Tester.Attributes;

namespace MapperGen.Tester.Models
{
    [GenerateMappedDto]
    public class Tiger
    {
        public string Name { get; set; }
        public string Specie { get; set; }

    }
}
