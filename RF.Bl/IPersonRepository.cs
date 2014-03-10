using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL.Model;  

namespace RF.BL
{
    public interface IPersonRepository
    {
        Person GetById(Guid id);
        Person Create(string ln, string fn, string sn);


        Zl GetZl(int kodps);
        void LinkZl(Person p, Zl zl);
    }
}
