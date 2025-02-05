using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Models.DataModels
{
    public  class mBase
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ParentId { get; set; } = string.Empty;

        public bool IsUsed { get; set; } = true;

        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? UpdateDate { get; set; } = null;
        public DateTime? DeletedDate { get; set; } = null;
    }
}
