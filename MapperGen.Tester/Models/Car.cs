using MapperGen.Tester.Attributes;

namespace MapperGen.Tester.Models
{
    [GenerateMappedDto]
    public class Car
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string SuperSecretProperty { get; set; }
    }
}
