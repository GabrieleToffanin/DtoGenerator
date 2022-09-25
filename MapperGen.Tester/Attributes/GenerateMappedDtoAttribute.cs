namespace MapperGen.Tester.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class GenerateMappedDtoAttribute : Attribute
    {
        public GenerateMappedDtoAttribute()
        {

        }
    }
}
