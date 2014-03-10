using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

namespace RF.WinApp.Svc.Controllers
{
    public class ValuesController : EntitySetController<TestDataObj, int>
    {
        // GET api/values
        //public IEnumerable<string> Get() {    return new string[] { "value1", "value2" };       }

        [Queryable]
        public IQueryable<TestDataObj> GetValues()
        {
            return new TestDataView().list.AsQueryable();
        }

        public override HttpResponseMessage Get(int key)
        {
            var test = new TestDataView().list.SingleOrDefault(s => s.ID == key);
            if (test == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, test);
            }
        }

         // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

         //// DELETE api/values/5
        //public override void Delete(int id)
        //{
        //}
    }

    public class TestDataView  
    {
        public List<TestDataObj> list = new List<TestDataObj>();
        public TestDataView()
        {
            list.Add(new TestDataObj() { ID = 1, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 2, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 3, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 4, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 5, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 6, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 7, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 8, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 9, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 12, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 13, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 14, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 21, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 22, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 23, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 24, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 31, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 32, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 33, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 34, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 41, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 42, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 43, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 44, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 45, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 46, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 47, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 48, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 49, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 112, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 113, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 114, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 121, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 122, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 123, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 124, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 131, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 132, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 133, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 134, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 51, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 52, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 53, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 54, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 55, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 56, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 57, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 58, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 59, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 512, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 513, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 514, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 521, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 522, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 523, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 524, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 531, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 532, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 533, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 534, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 541, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 542, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 543, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 544, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 545, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 546, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 547, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 548, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 549, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 5112, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 5113, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 5114, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 5121, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 5122, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 5123, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 5124, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 5131, Name = "DDDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 5132, Name = "DhghfhDDDD", IsTrue = true });
            list.Add(new TestDataObj() { ID = 5133, Name = "ВАЕНННННяD", IsTrue = false });
            list.Add(new TestDataObj() { ID = 5134, Name = "DDDDDD", IsTrue = true });
        }
    }

    public class TestDataObj
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsTrue { get; set; }
    }
}