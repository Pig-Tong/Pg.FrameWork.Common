using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pg.FrameWork.Common.Code;

namespace Pg.FrameWork.Common
{
    public class ImgResult : Result
    {
        public string FilePath
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string FileType
        {
            get;
            set;
        }

        public ImgResult(bool isSuccess)
            : base(isSuccess)
        {
        }
    }
}
