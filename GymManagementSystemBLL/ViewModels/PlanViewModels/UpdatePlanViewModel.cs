using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        [Required(ErrorMessage ="Plan Name Is Required")]
        [StringLength(50,ErrorMessage ="PlanName MustBe Less Than 51")]
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Description Is Required")]
        [StringLength(200,MinimumLength =5, ErrorMessage = "Description Must Be Between 5 And 200")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "DurationDays Is Required")]
        [Range(1,365,ErrorMessage = "DurationDays Must Be Between 1 And 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price Is Required")]
        [Range(0.1, 10000, ErrorMessage = "Price Must Be Between 0.1 And 10000")]
        public decimal Price { get; set; }
    }
}
