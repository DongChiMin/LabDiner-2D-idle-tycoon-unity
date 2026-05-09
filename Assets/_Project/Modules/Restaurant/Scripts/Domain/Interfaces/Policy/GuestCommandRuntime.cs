namespace LabDiner.Restaurant
{
    public class GuestCommandRuntime
    {
        public bool IsCancelled { get; private set; }

        public void Cancel() => IsCancelled = true;
    }
}