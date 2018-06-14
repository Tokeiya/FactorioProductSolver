using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FPS.CoreLib.Parser
{

	//こいつはキーバリュ||バリュ||コレクションを表現する。
	//てことは、当然即した実装は必須。
	public interface IElement
	{
		IEnumerable<IElement> Traverse();
		IEnumerable<IElement> Child();
	}


}
