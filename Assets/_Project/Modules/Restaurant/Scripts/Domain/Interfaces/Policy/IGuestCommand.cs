using System.Collections;
using LabDiner.Restaurant.Humanoid;

namespace LabDiner.Restaurant.Interface
{
    public interface IGuestCommand
    {
        string Name { get; }
        IEnumerator Execute(GuestContext ctx, GuestCommandRuntime runtime);
    }
}