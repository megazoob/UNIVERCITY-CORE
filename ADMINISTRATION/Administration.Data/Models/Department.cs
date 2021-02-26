using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Administration.Data.Models
{/// <summary>
/// Отдел.
/// </summary>
    public class Department
    {
        /// <summary></summary>
        public int Id { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Количество штатых единиц.
        /// </summary>
        [Required(ErrorMessage = "Number Of Staff Units Required")]
        [Display(Name = "Number Of Staff Units")]
        public int NumberOfStaffUnits { get; set; }

        /// <summary>
        /// Когда создан.
        /// </summary>
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Упразднен.
        /// </summary>
        [Display(Name="Abolished")]
        public Boolean Abolished { get; set; }

        /// <summary>
        /// Когда упразднен.
        /// </summary>
        [Display(Name = "Abolished Date")]
        public DateTime AbolishedDate { get; set; }


        /// <summary></summary>
        public Department SubordinateTo { get; set; }

        /// <summary>
        /// Parent Id.
        /// </summary>
        [Display(Name = "Subordinate To")]
        public int SubordinateToId { get; set; }

        /// <summary>
        /// Отделы в подчинении.
        /// </summary>
        [Display(Name="Manages Departments")]
        public ICollection<Department> ManagesDepartments { get; set; }

        /// <summary>/// </summary>
        public Department ()
        {
            CreatedDate = DateTime.Now;
            Abolished = false;
            Name = string.Empty;
            NumberOfStaffUnits = 1;
            ManagesDepartments = new List<Department>();
        }

    }
}
