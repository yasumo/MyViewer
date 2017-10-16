using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace MyViewerLib
{


    namespace LinqTest
    {
        [Table(Name = "TestTable")]
        public class TTestData
        {
            //ID
            [Column(Name = "Id", DbType = "INT", CanBeNull = true, IsPrimaryKey = false)]
            public Int32? Id { get; set; }

            // Title
            [Column(Name = "Title", DbType = "NVARCHAR", CanBeNull = true)]
            public String Title { get; set; }

            // Detail
            [Column(Name = "Detail", DbType = "NVARCHAR", CanBeNull = true)]
            public String Detail { get; set; }
        }
    }
}
