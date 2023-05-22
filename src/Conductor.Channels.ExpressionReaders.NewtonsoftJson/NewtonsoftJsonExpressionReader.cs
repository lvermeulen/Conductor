using Conductor.Abstractions;
using Newtonsoft.Json.Linq;

namespace Conductor.Channels.ExpressionReaders.NewtonsoftJson
{
    public abstract class NewtonsoftJsonExpressionReader : IExpressionReader
    {
        private readonly JObject _document;

        protected NewtonsoftJsonExpressionReader(JObject document)
        {
            _document = document;
        }

        public string ReadExpression(string expression)
        {
            foreach (var (key, value) in _document)
            {
                if (key == expression)
                {
                    return value?.Value<string>();
                }

                if (value is JObject jobj && jobj.TryGetValue(expression, out var jobjValue))
                {
                    return jobjValue.Value<string>();
                }
            }

            return null;
        }
    }
}
