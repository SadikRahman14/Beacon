// ViewModels/VoteResultVm.cs
namespace Beacon.ViewModels
{
    public class VoteResultVm
    {
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public int Score => Upvotes - Downvotes;
        public int UserVote { get; set; } 
    }
}
