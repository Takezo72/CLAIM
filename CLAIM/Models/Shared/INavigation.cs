using CLAIM.Models.Shared;

namespace CLAIM.Models.Shared
{
    public interface INavigation
    {
        string PreviousStep { get; }
        string NextStep { get; }
        ButtonListModel NavigationButtons { get; }
    }
}

