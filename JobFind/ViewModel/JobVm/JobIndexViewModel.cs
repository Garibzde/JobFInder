using JobFind.Models;
using JobFind.ViewModel.Category;
using JobFind.ViewModel.Job_Announcements;
using JobFind.ViewModel.Nature;
using JobFind.ViewModel.Pagination;

namespace JobFind.ViewModel.JobVm
{
        public class JobIndexVM
        {
            public List<GetCategoryVM>? Categories { get; set; }
            public List<GetAdminNatureVM> Natures { get; set; }
            public IEnumerable<GetJobVM> Jobs { get; set; }
                public PaginationInfoVM PaginationInfo { get; set; }
        public string SearchString { get; set; }
        public string LocationFilter { get; set; }
        public int SelectedCategoryId { get; set; }
        public int SelectedNatureId { get; set; }

    }
}

