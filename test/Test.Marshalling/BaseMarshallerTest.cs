using Mielek.Marshalling;

namespace Mielek.Test.Marshalling;

public abstract class BaseMarshallerTest
{
    protected StringWriter DataWriter { get; set; }
    protected string WrittenText
    {
        get
        {
            Marshaller.Flush();
            return DataWriter.ToString();
        }
    }
    protected Marshaller Marshaller { get; set; }

    [TestInitialize]
    public void Setup()
    {
        DataWriter = new StringWriter();
        Marshaller = Marshaller.Create(DataWriter);
    }

    [TestCleanup]
    public void Cleanup()
    {
        Marshaller.Dispose();
        DataWriter.Dispose();
    }
}