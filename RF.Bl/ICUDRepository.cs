using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RF.BL.Model;

namespace RF.BL
{
    public interface ICUDRepository<TModel> where TModel : BaseModel
    {
        object Create(TModel o);

        void Delete(TModel o);

        void Update(TModel o);
    }
}
