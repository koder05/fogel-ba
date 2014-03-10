using System;
using System.Threading;

namespace Common.Async
{
	public struct WorkItemId
	{
		public Guid BranchId;
		public int ItemId;

		public WorkItemId(Guid branchId, int itemId)
		{
			this.BranchId = branchId;
			this.ItemId = itemId;
		}

		public static bool operator ==(WorkItemId c1, WorkItemId c2)
		{
			return c1.BranchId == c2.BranchId && c1.ItemId == c2.ItemId;
		}

		public static bool operator !=(WorkItemId c1, WorkItemId c2)
		{
			return c1.BranchId != c2.BranchId || c1.ItemId != c2.ItemId;
		}

		public override bool Equals(object obj)
		{
			if (obj != null)
			{
				return this == (WorkItemId)obj;
			}
			
			return false;
		}

		public override int GetHashCode()
		{
			return this.BranchId.GetHashCode() ^ this.ItemId.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("{0}_{1}", this.BranchId, this.ItemId);
		}
	}
	
	public enum WorkItemStatus
	{
		Completed, Queued, Executing, Aborted
	}

	public sealed class WorkItem
	{
		private WaitCallback _callback;
		private IAsyncResult _state;
		private ExecutionContext _ctx;
		private WorkItemId _taskID;

		public WorkItem(WorkItemId taskID, WaitCallback wc, IAsyncResult state, ExecutionContext ctx)
		{
			_callback = wc;
			_state = state;
			_ctx = ctx;
			_taskID = taskID;
		}

		public WaitCallback Callback { get { return _callback; } }
		public IAsyncResult State { get { return _state; } }
		public ExecutionContext Context { get { return _ctx; } }
		public WorkItemId TaskID { get { return _taskID; } }
	}

	public class WorkItemAsyncState
	{
		public WorkItemId Id;
		public object State;

		public WorkItemAsyncState(WorkItemId id, object state)
		{
			this.Id = id;
			this.State = state;
		}
	}
}
