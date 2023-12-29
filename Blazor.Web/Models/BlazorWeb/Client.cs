using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor.Web.Models.BlazorWeb
{
    [Table("Client", Schema = "dbo")]
    public partial class Client
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public int? Number { get; set; }

        [Column(TypeName="datetime2")]
        public DateTime? DateCreated { get; set; }

        [Column(TypeName="datetime2")]
        public DateTime? DateDelete { get; set; }

        public bool? Active { get; set; }

        [Column(TypeName="datetime2")]
        public DateTime? DateModificated { get; set; }

        public string Email { get; set; }

    }
}