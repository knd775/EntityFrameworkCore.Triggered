﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Triggered.Tests.Stubs
{
    public class TriggerSessionStub : ITriggerSession
    {
        public int RaiseBeforeSaveTriggersCalls;
        public int RaiseAfterSaveTriggersCalls;
        public int RaiseAfterSaveFailedTriggersCalls;
        public int CaptureDiscoveredChangesCalls;
        public int DiscoverChangesCalls;

        public void CaptureDiscoveredChanges()
        {
            CaptureDiscoveredChangesCalls += 1;
        }

        public void DiscoverChanges()
        {
            DiscoverChangesCalls += 1;
        }

        public void Dispose()
        {
        }

        public Task RaiseAfterSaveFailedTriggers(Exception exception, CancellationToken cancellationToken = default)
        {
            RaiseAfterSaveFailedTriggersCalls += 1;
            return Task.CompletedTask;
        }

        public Task RaiseBeforeSaveTriggers(CancellationToken cancellationToken = default)
        {
            RaiseBeforeSaveTriggersCalls += 1;
            return Task.CompletedTask;
        }

        public Task RaiseAfterSaveTriggers(CancellationToken cancellationToken = default)
        {
            RaiseAfterSaveTriggersCalls += 1;
            return Task.CompletedTask;
        }

        public Task RaiseAfterSaveFailedTriggers(CancellationToken cancellationToken = default)
        {
            RaiseAfterSaveFailedTriggersCalls += 1;
            return Task.CompletedTask;
        }

        public Task RaiseBeforeSaveTriggers(bool skipDetectedChanges = false, CancellationToken cancellationToken = default)
        {
            RaiseBeforeSaveTriggersCalls += 1;
            return Task.CompletedTask;
        }
    }
}
