using System;
using System.Runtime.Serialization;

namespace FPS.CoreLib
{
	[Serializable]
	public class InconsistencyException : Exception
	{
		public InconsistencyException()
		{
		}

		public InconsistencyException(string message)
			: base(message)
		{
		}

		public InconsistencyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected InconsistencyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}