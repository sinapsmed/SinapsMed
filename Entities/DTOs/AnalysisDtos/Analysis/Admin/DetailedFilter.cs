namespace Entities.DTOs.AnalysisDtos.Analysis.Admin
{
    public class DetailedFilter
    {
        public List<Guid>? CategoryIds { get; set; }
        public string? Search { get; set; } 
        private int _page;
        public int Page
        {
            get => _page == 0 ? 1 : _page;
            set => _page = value;
        }

        private int _limit;
        public int Limit
        {
            get => _limit == 0 ? 10 : _limit;
            set => _limit = value;
        }
    }
}