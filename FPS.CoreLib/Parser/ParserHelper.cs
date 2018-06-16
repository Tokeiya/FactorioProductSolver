using Parseq;
using Parseq.Combinators;

namespace FPS.CoreLib.Parser
{
	public static class ParserHelper
	{
		public static bool IsSuccess<TToken, T>(this IReply<TToken, T> reply) =>
			reply.Case((_, __) => false, (_, __) => true);

	}
}