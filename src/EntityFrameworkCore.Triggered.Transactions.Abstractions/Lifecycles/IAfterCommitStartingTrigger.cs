﻿using System.Threading;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Triggered.Transactions.Lifecycles
{
    public interface IAfterCommitStartingTrigger
    {
        void AfterCommitStarting();
    }
}
