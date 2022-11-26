using System.Xml;

using Mielek.Marshalling.Expressions;
using Mielek.Marshalling.Policies;
using Mielek.Model;
using Mielek.Model.Expressions;
using Mielek.Model.Policies;

namespace Mielek.Marshalling;
public class Marshaller : IVisitor, IAsyncDisposable, IDisposable
{
    static readonly XmlWriterSettings XmlWriterSettings = new() { OmitXmlDeclaration = true, ConformanceLevel = ConformanceLevel.Fragment };
    static readonly Dictionary<Type, IMarshallerHandler> Handlers = new Dictionary<Type, IMarshallerHandler>()
    {
        #region Roots
        { typeof(PolicyDocument), new PolicyDocumentHandler() },
        { typeof(PolicyFragment), new PolicyFragmentHandler() },
        #endregion Roots

        #region Policies
        { typeof(BasePolicy), new BasePolicyHandler() },
        { typeof(SetBodyPolicy), new SetBodyPolicyHandler() },
        { typeof(SetHeaderPolicy), new SetHeaderPolicyHandler() },
        { typeof(SetMethodPolicy), new SetMethodPolicyHandler() },
        { typeof(SetStatusPolicy), new SetStatusPolicyHandler() },
        { typeof(CheckHeaderPolicy), new CheckHeaderPolicyHandler() },
        { typeof(RateLimitPolicy), new RateLimitPolicyHandler() },
        { typeof(RateLimitByKeyPolicy), new RateLimitByKeyPolicyHandler() },
        { typeof(GetAuthorizationContextPolicy), new GetAuthorizationContextPolicyHandler() },
        { typeof(IncludeFragmentPolicy), new IncludeFragmentPolicyHandler() },
        #endregion Policies

        #region Expressions
        { typeof(ConstantExpression), new ConstantExpressionHandler() },
        { typeof(InlineScriptExpression), new InlineScriptExpressionHandler() },
        { typeof(FileScriptExpression), new FileScriptExpressionHandler() },
        { typeof(FunctionFileScriptExpression), new FunctionFileScriptExpressionHandler() },
        #endregion Expressions
    };

    #region Create
    public static Marshaller Create(XmlWriter xmlWriter) => new Marshaller(xmlWriter, MarshallerOptions.Default);
    public static Marshaller Create(XmlWriter xmlWriter, MarshallerOptions options) => new Marshaller(xmlWriter, options);
    public static Marshaller Create(Stream output) => new Marshaller(XmlWriter.Create(output, XmlWriterSettings), MarshallerOptions.Default);
    public static Marshaller Create(Stream output, MarshallerOptions options) => new Marshaller(XmlWriter.Create(output, XmlWriterSettings), options);
    public static Marshaller Create(TextWriter output) => new Marshaller(XmlWriter.Create(output, XmlWriterSettings), MarshallerOptions.Default);
    public static Marshaller Create(TextWriter output, MarshallerOptions options) => new Marshaller(XmlWriter.Create(output, XmlWriterSettings), options);
    public static Marshaller Create(string outputFileName) => new Marshaller(XmlWriter.Create(outputFileName, XmlWriterSettings), MarshallerOptions.Default);
    public static Marshaller Create(string outputFileName, MarshallerOptions options) => new Marshaller(XmlWriter.Create(outputFileName, XmlWriterSettings), options);
    #endregion Create

    internal InternalWriter Writer { get; init; }

    Marshaller(XmlWriter xmlWriter, MarshallerOptions options)
    {
        this.Writer = new InternalWriter(xmlWriter, this);
        this.Options = options;
    }

    public MarshallerOptions Options { get; }

    public void Flush() => Writer.BaseWriter.Flush();

    public void Dispose() => Writer.BaseWriter.Dispose();

    public ValueTask DisposeAsync() => Writer.BaseWriter.DisposeAsync();

    public void Visit<T>(T element) where T : IVisitable
    {
        if (!Handlers.TryGetValue(typeof(T), out var handler))
        {
            throw new Exception();
        }

        handler.Marshal(this, element);
    }

    internal class InternalWriter
    {
        readonly Marshaller _marshaller;
        internal XmlWriter BaseWriter;

        internal InternalWriter(XmlWriter writer, Marshaller marshaller)
        {
            BaseWriter = writer;
            _marshaller = marshaller;
        }

        internal void WriteElement(string name) => BaseWriter.WriteElementString(name, null);

        internal void WriteStartElement(string name) => BaseWriter.WriteStartElement(name);

        internal void WriteEndElement() => BaseWriter.WriteEndElement();

        internal void WriteAttribute(string name, string value) => BaseWriter.WriteAttributeString(name, value);

        internal void WriteExpressionAsAttribute(string name, IExpression expression)
        {
            BaseWriter.WriteStartAttribute(name);
            expression.Accept(_marshaller);
            BaseWriter.WriteEndAttribute();
        }

        internal void WriteExpressionAsElement(string name, IExpression expression)
        {
            BaseWriter.WriteStartElement(name);
            expression.Accept(_marshaller);
            BaseWriter.WriteEndElement();
        }

        internal void WriteExpression(IExpression expression) => expression.Accept(_marshaller);

        internal void WriteString(string value) => BaseWriter.WriteString(value);
    }
}