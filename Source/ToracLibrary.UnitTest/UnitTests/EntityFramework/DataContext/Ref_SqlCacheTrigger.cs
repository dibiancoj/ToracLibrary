namespace ToracLibrary.UnitTest.EntityFramework.DataContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ref_SqlCacheTrigger
    {


        public int Id { get; set; }

        [Required]
        public DateTime LastUpdatedDate { get; set; }

    }
}