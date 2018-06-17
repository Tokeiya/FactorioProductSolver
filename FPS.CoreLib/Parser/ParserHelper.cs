using Parseq;

namespace FPS.CoreLib.Parser
{
	public static class ParserExtension
	{
		public static bool IsSuccess<TToken, T>(this IReply<TToken, T> reply) 
			=> reply.Case((_, __) => false, (_, __) => true);
	}



}