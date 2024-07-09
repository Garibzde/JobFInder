using JobFind.ViewModel.Category;
using JobFind.ViewModel.Job_Announcements;
using JobFind.ViewModel.Slider;

namespace JobFind.ViewModel.Home
{
    public class HomePageViewModel
    {
        public IEnumerable<GetCategoryVM> getCategoryVMs { get; set; }
        public IEnumerable<GetSliderVM> getSliderVMs { get; set; }

        public IEnumerable<GetUserJobVM> getUserJobVMs { get; set; }
       

        public int? SelectedCategoryId { get; set; }


    }
}
