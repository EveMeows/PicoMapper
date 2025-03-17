using MonoGayme.Core.UI;

namespace MonoGayme.Core.Controllers;

public class UpdatedEventArgs(IElement element)
{
	public IElement Element = element;
}
