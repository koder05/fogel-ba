using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Objects;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using EF.Poco;
using RF.Common.Logging;

namespace EF
{
	public class EFContext : DbContext
	{
		public EFContext()
			: base("MainDb2")
		{
			this.Configuration.LazyLoadingEnabled = true;
			this.Configuration.AutoDetectChangesEnabled = true;
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 180;
            this.Database.Connection.StateChange += Connection_StateChange;
		}

        private void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand("set ARITHABORT ON", sender as SqlConnection);
                cmd.ExecuteNonQuery();
            }
        }

		public DbSet<PersonPoco> Persons { get; set; }
        public DbSet<ZlPoco> Zls { get; set; }
		public DbSet<UserPoco> Users { get; set; }
        public DbSet<PolicyGroupPoco> PolicyGroups { get; set; }
        public DbSet<PolicyPoco> Policies { get; set; }
		public DbSet<PswChReqPoco> PswChRequests { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new PersonCfg());
            modelBuilder.Configurations.Add(new ZlCfg());
			modelBuilder.Configurations.Add(new UserCfg());
            modelBuilder.Configurations.Add(new PolicyGroupCfg());
            modelBuilder.Configurations.Add(new PolicyCfg());
			modelBuilder.Configurations.Add(new PswChReqCfg());
		}

		public ObjectStateEntry FindEntityById<TPoco>(Guid id) where TPoco : BasePoco, new()
		{
			ObjectStateEntry entry;
			ObjectContext context = ((IObjectContextAdapter)this).ObjectContext;
			context.ObjectStateManager.TryGetObjectStateEntry(GetEntityKey(typeof(TPoco), context, id), out entry);
			return entry;
		}

		private EntityKey GetEntityKey(Type type, ObjectContext context, Guid id)
		{
			// Thanks to Kevin McNeish for this little gem
			string entityTypeName = type.Name;
			var container = context.MetadataWorkspace.GetEntityContainer(
				context.DefaultContainerName, System.Data.Metadata.Edm.DataSpace.CSpace);
			string entitySetName = (from meta in container.BaseEntitySets
									where meta.ElementType.Name == entityTypeName
									select meta.Name).First();
			return new EntityKey(string.Format("{0}.{1}", typeof(EFContext).Name, entitySetName), "Id", id);
		}

        public override int SaveChanges()
        {
            IEnumerable<Tuple<string, string, object, string, object, object>> log = this.GetSavedValues();

            Logs.LogDb.Log(log, null, null);  

            int ret = base.SaveChanges();
            //if (ret > 0) LogDb;
            return ret;
        }
	}
}
