using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using EF;
using EF.Poco;
using RF.BL.EF.PocoProxy; 
using RF.BL.Model; 

namespace RF.BL.EF.Repositories
{
    public class PersonRepository : CUDRepository<Person>, IPersonRepository 
    {
        private EFContext _db;

        public PersonRepository(EFContext db)
            : base(db)
        {
            _db = db;
        }

        public Person GetById(Guid id)
        {
            PersonPoco poco = (from p in _db.Persons where p.Id==id select p).FirstOrDefault();
            if (poco != null)
                return ProxyActivator.CreateProxy<PersonPoco, Person>(poco);

            return null;
        }

        public Person Create(string ln, string fn, string sn)
        {
            Person p = ProxyActivator.CreateProxy<PersonPoco, Person>(new PersonPoco() {LN = ln, FN = fn, SN = sn });
            this.CUD<PersonPoco>(p, EntityState.Added);
            return p;
        }

        public Zl GetZl(int kodps)
        {
            ZlPoco poco = (from zl in _db.Zls where zl.KodPs == kodps select zl).FirstOrDefault();
            return ProxyActivator.CreateProxy<ZlPoco, Zl>(poco); 
        }

        public void LinkZl(Person p, Zl zl)
        {
            zl.Person = p;
            this.CUD<Zl, ZlPoco>(zl, EntityState.Modified);
        }
    }
}
