//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace GlobalSchoolSearch2017_Application.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class School
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public School()
        {
            this.Reviews = new HashSet<Review>();
        }
    
        public int SchoolId { get; set; }

        [Required]
        [Display(Name ="School")]
        public string SchoolName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "City")]
        public string CityName { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string CountryName { get; set; }

        [Display(Name = "Contact No.")]
        public string Contact_No_ { get; set; }

        [Display(Name = "Contact No. 2")]
        public string Alternate_Contact_No_ { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        [Display(Name = "Examination Board")]
        public string Examination_Board { get; set; }

        [Display(Name = "Type of School")]
        public string Type_of_School { get; set; }

        [Display(Name = "Admission Criteria")]
        public string Admission_Criteria { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Admission Start Date")]
        public Nullable<System.DateTime> Admission_Start_Date { get; set; }

        [Display(Name = "Application Form")]
        public string Application_Form { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "submission Date")]
        public Nullable<System.DateTime> Submission_Date { get; set; }

        [Display(Name = "Additional Information")]
        public string Additional_Information_ { get; set; }

        [Required]
        [Display(Name = "Authorizer Email")]
        public string Authorizer_Email { get; set; }

        public Nullable<double> Rating { get; set; }

        [Required]
        [Display(Name = "Date of Updation")]
        public System.DateTime Date_of_Updatation { get; set; }
    
        public virtual City City { get; set; }

        public virtual Country Country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}