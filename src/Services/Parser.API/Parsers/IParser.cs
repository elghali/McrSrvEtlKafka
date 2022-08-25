
namespace Parser.API.Parsers
{
    internal interface IParser
    {
        void ParserData(CancellationToken cancellationToken);
    }
}
