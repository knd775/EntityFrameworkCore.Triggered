﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggered.Internal;
using EntityFrameworkCore.Triggered.Tests.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EntityFrameworkCore.Triggered.Tests.Internal
{
    public class ApplicationTriggerServiceProviderAccessorTests
    {
        class TestDbContext : DbContext
        {
            public TestDbContext()
            {
                    
            }

            public TestDbContext(DbContextOptions options) : base(options)
            {
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);

                optionsBuilder.UseInMemoryDatabase("test");
                optionsBuilder.UseTriggers(triggerOptions => {
                    triggerOptions.AddTrigger<TriggerStub<object>>();
                });
            }
        }


        [Fact]
        public void GetTriggerServiceProvider_NoApplicationDi_ReturnsScopedInternal()
        {
            var dbContext = new TestDbContext();

            var subject = new ApplicationTriggerServiceProviderAccessor(dbContext.GetInfrastructure(), null);
            var scopedObject1 = subject.GetTriggerServiceProvider().GetService<IBeforeSaveTrigger<object>>();
            Assert.NotNull(scopedObject1);

            var scopedObject2 = subject.GetTriggerServiceProvider().GetService<IBeforeSaveTrigger<object>>();
            Assert.NotNull(scopedObject2);

            Assert.NotEqual(scopedObject1, scopedObject2);
        }

        [Fact]
        public void GetTriggerServiceProvider_WithApplicationDi_ReturnsScopedApplication()
        {
            var applicationServiceProvider = new ServiceCollection()
                .AddDbContext<TestDbContext>(options => {
                    options.UseInMemoryDatabase("Test")
                           .UseTriggers();
                })
                .AddScoped<object>()
                .BuildServiceProvider();

            var dbContext = applicationServiceProvider.GetRequiredService<TestDbContext>();

            var subject = new ApplicationTriggerServiceProviderAccessor(dbContext.GetInfrastructure(), null);
            var triggerServiceProvider = subject.GetTriggerServiceProvider();

            var scopedObject = triggerServiceProvider.GetService<object>();
            Assert.NotNull(scopedObject);

            var applicationScopedObject = applicationServiceProvider.GetService<object>();
            Assert.NotNull(applicationScopedObject);
            Assert.NotEqual(applicationScopedObject, scopedObject);
        }

        [Fact]
        public void GetTriggerServiceProvider_WithApplicationDiAndTransform_ReturnsCustomServiceProvider()
        {
            var applicationServiceProvider = new ServiceCollection()
                .AddDbContext<TestDbContext>(options => {
                    options.UseInMemoryDatabase("Test")
                           .UseTriggers();
                })
                .AddScoped<object>()
                .BuildServiceProvider();

            var dbContext = applicationServiceProvider.GetRequiredService<TestDbContext>();

            var subject = new ApplicationTriggerServiceProviderAccessor(dbContext.GetInfrastructure(), _ => applicationServiceProvider);
            var triggerServiceProvider = subject.GetTriggerServiceProvider();

            Assert.Equal(applicationServiceProvider, triggerServiceProvider);

        }
    }
}