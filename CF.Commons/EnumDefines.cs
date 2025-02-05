using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Commons
{
 
    public enum eLineType
    {
        [Description("-")]
        None = -1,

        [Description("")]
        Empty = 0,
        [Description("Text")]
        NormalText = 10,


        [Description("#")]
        Heading1 = 101,
        [Description("##")]
        Heading2 = 102,
        [Description("###")]
        Heading3 = 103,
        [Description("####")]
        Heading4 = 104,
        [Description("#####")]
        Heading5 = 105,
        [Description("######")]
        Heading6 = 106,
        [Description("#######")]
        Heading7 = 107,

        
    }

    public enum eCheckStatus
    {
        None = -1,

        [Description("완료")]
        Completed  = 10,
        [Description("보류")]
        Hold = 20,

        [Description("실패")]
        Fail = 70,
    }
}
