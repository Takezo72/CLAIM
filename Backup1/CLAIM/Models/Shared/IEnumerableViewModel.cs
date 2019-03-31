namespace CLAIM.Models.Shared
{
    public interface IEnumerableViewModel : IViewModel
    {
        bool IsShown { get; set; }
        string DeleteLabel { get; }
        string AddLabel { get; }
    }
}